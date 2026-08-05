using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ballon : MonoBehaviour
{
    public float ballonGravityScale;

    Rigidbody2D rb;

    private void Start()
    {
        ballonGravityScale = GetComponent<Rigidbody2D>().gravityScale;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
