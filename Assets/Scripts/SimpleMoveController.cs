using UnityEngine;

public class SimpleMoveController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector2 moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            moveDirection += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            moveDirection -= Vector2.up;
        if (Input.GetKey(KeyCode.A))
            moveDirection -= Vector2.right;
        if (Input.GetKey(KeyCode.D))
            moveDirection += Vector2.right;

        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed *= sprintMultiplier;

        rb.MovePosition(rb.position + moveDirection.normalized * currentSpeed * Time.fixedDeltaTime);
    }
}