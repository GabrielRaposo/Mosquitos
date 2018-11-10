using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Gameplay Components")]
    public Rescuee rescuee;
    public LayerMask whatIsRescuee;
    public float checkRange;

    [Header("Manager Reference")]
    public GameManager gameManager;

    private FollowMouse followMouse;
    private FollowTouch followTouch;

    private enum State { None, Held, Exit }
    private State state;

    private void Awake()
    {
        followMouse = GetComponent<FollowMouse>();
        followTouch = GetComponent<FollowTouch>();
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.None:
                if (Input.GetMouseButtonDown(0))
                {
                    if (CheckForRescuee(transform.position)) {
                        rescuee.Follow(transform);
                    } 
                    state = State.Held;
                }

                if (Input.touchCount == 1)
                {
                    switch (Input.touches[0].phase)
                    {
                        case TouchPhase.Began:
                            if (CheckForRescuee(transform.position))
                            {
                                rescuee.Follow(transform);
                            }
                            state = State.Held;
                            break;
                    }
                }
                break;

            case State.Held:
                if (Input.GetMouseButtonUp(0))
                {
                    rescuee.Unfollow();
                    state = State.None;
                }
                if (Input.touchCount == 1)
                {
                    switch (Input.touches[0].phase)
                    {   
                        case TouchPhase.Ended:
                            rescuee.Unfollow();
                            state = State.None;
                            break;
                    }
                } else 
                if (Input.touchCount > 1)
                {
                    rescuee.Unfollow();
                    state = State.None;
                }
                break;
        }

        #region touch
        //if (Input.touchCount == 1)
        //{
        //    switch (Input.touches[0].phase)
        //    {
        //        case TouchPhase.Began:
        //            isPressed = true;
        //            break;
        //        case TouchPhase.Ended:
        //            isPressed = false;
        //            break;
        //    }
        //}
        #endregion
    }

    private bool CheckForRescuee(Vector2 position)
    {
        bool search = Physics2D.OverlapCircle(position, checkRange, whatIsRescuee);
        return search;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRange);
    }
}
