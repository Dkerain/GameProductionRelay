using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveController : MonoBehaviour
{
    public float moveSpeed = 5f;            // Ĭ���ƶ��ٶ�
    public float sprintMultiplier = 2f;     // ���ٱ��ʣ���סShiftʱ��
    public float rotateSpeed = 90f;         // ÿ����ת�Ƕȣ��ȣ�

    void Update()
    {
        HandleMovement();
    }

    /// <summary>
    /// ���� WASD �ƶ�
    /// </summary>
    void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            moveDirection += transform.up;
        if (Input.GetKey(KeyCode.S))
            moveDirection -= transform.up;
        if (Input.GetKey(KeyCode.A))
            moveDirection -= transform.right;
        if (Input.GetKey(KeyCode.D))
            moveDirection += transform.right;

        // Shift ����
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed *= sprintMultiplier;

        transform.position += moveDirection.normalized * currentSpeed * Time.deltaTime;
    }
}
