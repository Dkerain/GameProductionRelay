using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotationController : MonoBehaviour
{
    private Camera viewCamera;

    private void Start()
    {
        viewCamera = Camera.main;
        if (viewCamera == null)
        {
            Debug.LogError("No view camera found");
        }
    }

    private void Update()
    {
        if (viewCamera == null)
        {
            return;
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = viewCamera.nearClipPlane; // Set z to the near clip plane to get a valid point in 3D space
        Vector3 worldMousePosition = viewCamera.ScreenToWorldPoint(mousePosition);

        Vector2 direction = (worldMousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
