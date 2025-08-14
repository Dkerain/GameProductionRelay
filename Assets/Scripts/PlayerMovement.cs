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

    // ��Ӷ���Ұ���������
    private FieldOfView fieldOfView;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        inputAction = playerInput.actions["Move"];
        animator = GetComponent<Animator>();

        // ��ȡ��Ұ���
        fieldOfView = GetComponent<FieldOfView>();
    }

    void Update()
    {
        Vector2 move = inputAction.ReadValue<Vector2>();
        rigidbody.velocity = move * speed;

        // ���¶�������
        animator.SetFloat("Horizontal", move.x);
        animator.SetFloat("Vertical", move.y);
        animator.SetFloat("Speed", move.sqrMagnitude);

        // ֻ�����ƶ�ʱ�Ÿ��·���
        if (move != Vector2.zero)
        {
            animator.SetFloat("LastHorizontal", move.x);
            animator.SetFloat("LastVertical", move.y);

            // ������Ұ����
            UpdateFieldOfViewDirection(move);
        }
    }

    // �����ƶ����������Ұ����
    private void UpdateFieldOfViewDirection(Vector2 move)
    {
        if (fieldOfView == null) return;

        // ȷ����Ҫ�ƶ���������ˮƽ����
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

