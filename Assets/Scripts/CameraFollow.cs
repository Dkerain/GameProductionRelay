using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float transitionSpeed = 2;
    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position+offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, transitionSpeed * Time.deltaTime);
        }
    }
}
