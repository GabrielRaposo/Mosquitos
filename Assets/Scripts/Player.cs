using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

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
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            followMouse.enabled = true;
            gameManager.IsGameplayEnabled(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            followMouse.enabled = false;
            gameManager.IsGameplayEnabled(false);
        }
    }

    //private void OnMouseDown()
    //{
    //    followMouse.enabled = true;
    //    gameManager.IsGameplayEnabled(true);
    //}

    //private void OnMouseUp()
    //{
    //    followMouse.enabled = false;
    //    gameManager.IsGameplayEnabled(false);
    //}
}
