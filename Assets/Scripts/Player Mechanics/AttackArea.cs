using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] float forcePower = 10f;

    [SerializeField] PlayerController player;
    [SerializeField] ScoreManager score;

    public bool isHit = false;

    Rigidbody2D ballon;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ballon") && isHit == false)
        {
            isHit = true;
            ballon = collision.GetComponent<Rigidbody2D>();          

            Vector2 forceDirection = transform.up * forcePower;
            ballon.AddForce(forceDirection, ForceMode2D.Impulse);

            audioManager.PlaySFX(audioManager.ballonHit, 1);

            score.AddScore(player.ballonHitScore);
        }
    }

    private void OnDisable()
    {
        isHit = false;
    }
}
