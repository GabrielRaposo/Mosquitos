using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Gameplay Components")]
    public CircleCollider2D hurtbox;
    public GameObject hitbox;
    public SpriteRenderer damageVisual;

    [Header("Manager Reference")]
    public GameManager gameManager;

    FollowMouse followMouse;
    //FollowTouch followTouch;

    static public Player instance;

    private void Awake()
    {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        followMouse = GetComponent<FollowMouse>();

        SetInvincibility(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            followMouse.enabled = true;
            gameManager.IsPlayerActive(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            followMouse.enabled = false;
            gameManager.IsPlayerActive(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetInvincibility(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            SetInvincibility(false);
        }
    }

    //private void OnMouseDown()
    //{
    //    followMouse.enabled = true;
    //    gameManager.IsPlayerActive(true);
    //}

    //private void OnMouseUp()
    //{
    //    followMouse.enabled = false;
    //    gameManager.IsPlayerActive(false);
    //}

    public void SetInvincibility(bool value)
    {
        hurtbox.enabled = !value;
        hitbox.SetActive(value);
    }

    public void TakeDamage()
    {
        StartCoroutine(DamageEffect());

        //invincibility timer
    }

    IEnumerator DamageEffect()
    {
        followMouse.enabled = false;
        damageVisual.enabled = true;
        Time.timeScale = .3f;

        yield return new WaitForSeconds(.3f);

        followMouse.enabled = true;
        damageVisual.enabled = false;
        Time.timeScale = 1;
    }
}
