using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public EnemyPool enemyPool;
    public Player player;
    public Rescuee rescuee;
    public LayerMask whatIsRescuee;

    public GameObject holdRescueDisplay;
    public GameObject safeSpaceDisplay;
    public GameObject tutorialDisplay;

    private GameManager caller;
    private bool gettingPlayerInput;
	
    public void Call(GameManager caller)
    {
        this.caller = caller;
        StartCoroutine(TutorialSetup());
    }

	private IEnumerator TutorialSetup()
    {
        holdRescueDisplay.SetActive(false);
        safeSpaceDisplay.SetActive(false);
        player.enabled = true;
        rescuee.enabled = true;
        yield return new WaitForSeconds(.5f);

        tutorialDisplay.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        tutorialDisplay.SetActive(false);

        GameObject enemy = enemyPool.GetEnemy(new Vector2(-3, 8), 180);
        Transform enemyTransform = enemy.transform;
        yield return new WaitUntil(() => enemyTransform.position.y < 2);

        enemy.GetComponent<Animator>().enabled = false;
        Animator wingAnimator = enemy.transform.GetChild(0).GetComponent<Animator>();
        wingAnimator.enabled = false;
        enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        holdRescueDisplay.GetComponent<RectTransform>().position 
            = Camera.main.WorldToScreenPoint(rescuee.transform.position);
        holdRescueDisplay.SetActive(true);
        safeSpaceDisplay.SetActive(false);
        gettingPlayerInput = true;
        player.enabled = true;
        rescuee.enabled = true;

        yield return new WaitWhile(() => gettingPlayerInput);

        enemy.GetComponent<Enemy>().Launch();
        enemy.GetComponent<Animator>().enabled = true;
        wingAnimator.enabled = true;

        yield return new WaitUntil(() => enemyTransform.position.y < -3);

        yield return new WaitForSeconds(1f);
        caller.SetState(GameManager.State.Playing);
    }

    private void Update()
    {
        if (gettingPlayerInput)
        {
            if (holdRescueDisplay.activeSelf) {
                if (rescuee.isFollowing)
                {
                    holdRescueDisplay.SetActive(false);
                    safeSpaceDisplay.SetActive(true);
                }
            } else {
                if (!rescuee.isFollowing)
                {
                    holdRescueDisplay.GetComponent<RectTransform>().position
                        = Camera.main.WorldToScreenPoint(rescuee.transform.position);
                    holdRescueDisplay.SetActive(true);
                    safeSpaceDisplay.SetActive(false);
                }
            }

            if (safeSpaceDisplay.activeSelf)
            {
                bool search = Physics2D.OverlapCircle(Vector3.right * 3, 1.5f, whatIsRescuee);
                if (search)
                {
                    gettingPlayerInput = false;
                    safeSpaceDisplay.SetActive(false);
                }
            }
        }
    }
}
