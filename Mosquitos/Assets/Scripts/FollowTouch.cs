using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTouch : MonoBehaviour {

    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        if (Input.touchCount < 1) return;

        Vector3 position = Input.touches[0].position;
        position.z = 10f;
        transform.position = mainCamera.ScreenToWorldPoint(position);
    }
}
