using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region player data
    // Player Variables
    [Header("Player Variables"), Space(5)]
    [Space(5)]
    [SerializeField] float moveSpeed = 5f;
    [Space(5)]
    [SerializeField] float jumpForce = 10f;
    [Space(5)]
    [SerializeField] float dashForce = 20f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 1f;
    [Space(5)]
    [SerializeField] float attackTime = 0.5f;
    [Space(5)]
    [SerializeField] float knockbackForce = 10f;
    [SerializeField] float upForce = 5f;
    [SerializeField] float dazedDuration = 3f;

    [Header("Score Settings")]
    [SerializeField] public int ballonHitScore = 100;
    [SerializeField] public int DisturberhitScore = -50;

    // Player Components
    [Header("Player Components"), Space(5)]
    [SerializeField] BoxCollider2D feetCollider;
    [SerializeField] GameObject attackBox;
    [SerializeField] GameObject attackIndicator;
    [SerializeField] AttackParent attackParent;
    [SerializeField] ScoreManager score;
    [SerializeField] Animator playerAnim;
    [SerializeField] Animator attackVFX;
    #endregion

    // Player State Variables
    float facingDirection;

    bool isDashing = false;
    bool canDash = true;
    bool canAttack = true;
    bool isHit;
    bool isDazed = false;
    bool isRunning = false;

    // Player Input Variables
    Rigidbody2D rb;
    Vector2 playerInput, pointerInput;
    Vector3 pointerPos;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackParent = GetComponentInChildren<AttackParent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing && !isDazed)
            PlayerRun();

        if (playerInput.x != 0)
            facingDirection = Mathf.Sign(playerInput.x);

        PointerPosition();

        AnimationHandler();

        PlayerFacing();
    }

    #region Input System Callbacks
    void OnMove(InputValue value)
    {
        playerInput = value.Get<Vector2>();
    }

    void OnDash(InputValue value)
    {
        if(value.isPressed && canDash == true && !isDazed)
        {
            StartCoroutine(PlayerDash());
        }
    }

    void OnJump(InputValue value)
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Arena")) || isDazed) { return; }
        if (value.isPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void OnAttack(InputValue value)
    {
        if (value.isPressed && canAttack == true &&  !isDazed)
        {
            StartCoroutine(PlayerAttack());
        }
    }

    void OnPointer(InputValue value)
    {
        pointerPos = value.Get<Vector2>();
    }

    void OnPause(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Hit Button Pause via PlayerController!");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PauseGame();
            }
        }
    }
    #endregion

    #region Player Mechanics
    void PlayerRun()
    {
        rb.linearVelocity = new Vector2(playerInput.x * moveSpeed, rb.linearVelocity.y);
        bool isMoving = Mathf.Abs(playerInput.x) > 0.01f;

        if (isMoving && !isRunning && feetCollider.IsTouchingLayers(LayerMask.GetMask("Arena")))
        {
            audioManager.PlayRunSFX(1.5f);
            isRunning = true;
        }
        else if ((!isMoving && isRunning) || !feetCollider.IsTouchingLayers(LayerMask.GetMask("Arena")))
        {
            audioManager.StopRunSFX();
            isRunning = false;
        }
    }

    void PointerPosition()
    {
        if(attackParent == null) { return; }

        Vector3 screenPos = pointerPos;
        screenPos.z = Camera.main.nearClipPlane;
        pointerInput = Camera.main.ScreenToWorldPoint(screenPos);
        attackParent.pointerPos = pointerInput;
    }

    private IEnumerator PlayerDash()
    {
        canDash = false;
        isDashing = true;
        var baseGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(facingDirection * dashForce, 0f);

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rb.gravityScale = baseGravity;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator PlayerAttack()
    {
        canAttack = false;
        attackParent.rotationLocked = true;

        attackIndicator.SetActive(false);
        attackBox.SetActive(true);

        attackVFX.SetTrigger("isAttack");

        yield return new WaitForSeconds(attackTime);
        attackBox.SetActive(false);
        attackIndicator.SetActive(true);

        attackParent.rotationLocked = false;
        canAttack = true;
    }

    private IEnumerator PlayerDazed()
    {
        isDazed = true;
        audioManager.StopRunSFX();
        yield return new WaitForSeconds(dazedDuration);
        isDazed = false;
    }

    void PlayerFacing()
    {
        bool flip = MathF.Abs(rb.linearVelocity.x) > Mathf.Epsilon;
        if (flip)
        {
            transform.localScale = new Vector2(MathF.Sign(rb.linearVelocity.x), 1f);
        }
    }

    void AnimationHandler()
    {
        // Animation for Running
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.01f;
        playerAnim.SetBool("isRunning", isRunning);

        // Animation for Jump
        bool isGrounded = feetCollider.IsTouchingLayers(LayerMask.GetMask("Arena"));
        bool isJumping = !isGrounded;
        playerAnim.SetBool("isJumping", isJumping);

        // Animation for Dazed
        playerAnim.SetBool("isDazed", isDazed);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Disturber") && !isDazed)
        {
            Rigidbody2D disturberRb = collision.GetComponentInParent<Rigidbody2D>();
            Debug.Log(disturberRb);
            Vector2 knockbackDir;

            if (disturberRb != null && disturberRb.linearVelocity.sqrMagnitude > 0.01f)
            {
                knockbackDir = disturberRb.linearVelocity.normalized; // arah gerak Disturber
            }
            else
            {
                knockbackDir = ((Vector2)transform.position - (Vector2)collision.transform.position).normalized; // fallback posisi
            }

            rb.linearVelocity = Vector2.zero; // reset dulu biar knockback konsisten
            Vector2 forceDir = new Vector2(knockbackDir.x * knockbackForce, upForce);
            rb.AddForce(forceDir, ForceMode2D.Impulse);

            Debug.Log("Player Hit!!, Direction" + forceDir);

            score.AddScore(DisturberhitScore);

            StartCoroutine(PlayerDazed());
        }
    }
}