using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public enum State { Tutorial, Playing, Paused }

    [Header("Manager")]
    public TutorialManager tutorialManager;
    public Spawner cicleManager;
    public SymptomPanel sintomsPanel;
    public ResultsScreen resultsScreen;

    [Space(20)]
    public int repellentIndex;
    public GameObject repellentPrefab;
    public Pattern[] stages;

    private int stageIndex;
    private bool waitingForRepellent;
    private bool hasSpawnedRepellent;
    private Rescuee rescuee;
    private SceneTransition transition;
    private State state;

	void Start ()
    {
        rescuee = Rescuee.instance;
        transition = SceneTransition.instance;
        SetState(State.Tutorial);
        //SetState(State.Playing);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)) StartCoroutine(TransitionToSintomsPanel());
    }

    public void SetState(State state)
    {
        this.state = state;
        switch (state)
        {
            case State.Tutorial:
                tutorialManager.Call(this);
                break;

            case State.Playing:
                CallStage();
                break;
        }
    }

    public void CallStage()
    {
        if (!hasSpawnedRepellent && stageIndex == repellentIndex)
        {
            StartCoroutine(SpawnRepellent());
            return;
        }

        if (waitingForRepellent) return;

        if (stageIndex < stages.Length) {
            Pattern current = stages[stageIndex];
            cicleManager.SetStage(current);
        } else {
            StartCoroutine(resultsScreen.Call());
        }
        stageIndex++;
    }
    
    public IEnumerator SpawnRepellent()
    {
        yield return new WaitForSeconds(4);
        GameObject repellent = Instantiate(repellentPrefab, Vector3.zero, Quaternion.identity, transform);
        repellent.SetActive(true);
        yield return repellent.GetComponent<Repellent>().SpawnAnimation();

        hasSpawnedRepellent = true;
    }

    public void GotRepellent()
    {
        waitingForRepellent = false;
        CallStage();
    }

    public void ReactToDamage()
    {
        switch (state)
        {
            case State.Tutorial:
                if (transition) transition.Call("GameScene");
                else SceneTransition.CallScene("GameScene");                
                break;

            case State.Playing:
                StartCoroutine(TransitionToSintomsPanel());
                break;
        }
    }

    private IEnumerator TransitionToSintomsPanel()
    {
        Time.timeScale = .1f;
        while(Time.timeScale > .002f)
        {
            yield return new WaitForEndOfFrame();
            Time.timeScale -= .002f;
        }
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(.3f);

        sintomsPanel.Call(this);
    }

    public void ReturnWithSymptom(Symptom symptom, bool endHere = false)
    {
        rescuee.SetSymptom(symptom);
        //rescuee.StartCoroutine(rescuee.InvincibilityState());
        rescuee.CallInvincibilityState();
        Time.timeScale = 1;
        if (endHere) stageIndex = repellentIndex;
    }
}
