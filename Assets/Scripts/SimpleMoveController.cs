using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveController : MonoBehaviour
{
    public float moveSpeed = 5f;            // 默认移动速度
    public float sprintMultiplier = 2f;     // 加速倍率（按住Shift时）
    public float rotateSpeed = 90f;         // 每秒旋转角度（度）

    void Update()
    {
        HandleMovement();
    }

    /// <summary>
    /// 处理 WASD 移动
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

        // Shift 加速
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed *= sprintMultiplier;

        transform.position += moveDirection.normalized * currentSpeed * Time.deltaTime;
    }
}
