using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rigidbody;
    private PlayerInput playerInput;
    private InputAction inputAction;
    private Animator animator;

    // 添加对视野组件的引用
    private FieldOfView fieldOfView;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        inputAction = playerInput.actions["Move"];
        animator = GetComponent<Animator>();

        // 获取视野组件
        fieldOfView = GetComponent<FieldOfView>();
    }

    void Update()
    {
        Vector2 move = inputAction.ReadValue<Vector2>();
        rigidbody.velocity = move * speed;

        // 更新动画参数
        animator.SetFloat("Horizontal", move.x);
        animator.SetFloat("Vertical", move.y);
        animator.SetFloat("Speed", move.sqrMagnitude);

        // 只有在移动时才更新方向
        if (move != Vector2.zero)
        {
            animator.SetFloat("LastHorizontal", move.x);
            animator.SetFloat("LastVertical", move.y);

            // 更新视野方向
            UpdateFieldOfViewDirection(move);
        }
    }

    // 根据移动方向更新视野方向
    private void UpdateFieldOfViewDirection(Vector2 move)
    {
        if (fieldOfView == null) return;

        // 确定主要移动方向（优先水平方向）
        if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
        {
            if (move.x > 0)
                fieldOfView.currentDirection = FieldOfView.Direction.Right;
            else
                fieldOfView.currentDirection = FieldOfView.Direction.Left;
        }
        else
        {
            if (move.y > 0)
                fieldOfView.currentDirection = FieldOfView.Direction.Up;
            else
                fieldOfView.currentDirection = FieldOfView.Direction.Down;
        }
    }
}

