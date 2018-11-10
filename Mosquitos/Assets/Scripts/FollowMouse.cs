using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;
    }
}
