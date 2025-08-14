using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rigidbody;
    private PlayerInput playerInput;
    private InputAction inputAction;
    private Animator animator;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        inputAction = playerInput.actions["Move"];
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = inputAction.ReadValue<Vector2>();
        rigidbody.velocity = move * speed;
        animator.SetFloat("Horizontal",move.x);
        animator.SetFloat("Vertical", move.y);
        animator.SetFloat("Speed", move.sqrMagnitude);
        if(move != Vector2.zero)
        {
            animator.SetFloat("LastHorizontal", move.x);
            animator.SetFloat("LastVertical", move.y);
        }
    }
}
