using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ballon : MonoBehaviour, IWindEffectable
{
    public float ballonGravityScale;

    float moveSpeed = 0f;
    float moveDirection;

    bool isBlowed = false;

    Rigidbody2D rb;

    private void Start()
    {
        ballonGravityScale = GetComponent<Rigidbody2D>().gravityScale;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isBlowed)
        {
            rb.linearVelocity = new Vector2(moveSpeed * moveDirection, rb.linearVelocity.y);

            return;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
    }

    public void WindBlow(float strength, float direction, bool condition)
    {
        isBlowed = condition;
        moveSpeed = strength;
        moveDirection = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
