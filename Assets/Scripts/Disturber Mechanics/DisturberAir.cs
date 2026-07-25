using System.Collections;
using UnityEngine;

public class DisturberAir : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform player;
    [Space(10)]

    [SerializeField] float strikeSpeed = 5f;
    [SerializeField] bool isStriking = false;
    [Space(5)]
    [SerializeField] float scanTime = 1f;
    [SerializeField] float lockTime = 1f;
    [Space(20)]

    [SerializeField] CapsuleCollider2D bodyCollider;
    [SerializeField] Transform disturbIndicator;
    [SerializeField] Transform bodySprite;
    [SerializeField] GameObject hitBox;
    [SerializeField] Animator airAnim;

    Vector3 playerPos;
    Vector3 lockedPlayerPos;

    float selfRotation;

    bool isScanning = false;
    bool isLockedOn = false;
    bool isAttacking = false;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        playerPos = player.position;

        // Selama belum lock, indicator terus ngikutin player real-time
        if (!isLockedOn)
        {
            RotateIndicatorTo(playerPos);
        }

        if (!isScanning && !isLockedOn && !isStriking)
        {
            isScanning = true;
            StartCoroutine(ScanPlayer());
        }

        if (isStriking)
        {
            Strikeplayer();
        }

        AnimationHandler();
    }

    void RotateIndicatorTo(Vector3 targetPos)
    {
        Vector2 direction = targetPos - disturbIndicator.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        disturbIndicator.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    void Strikeplayer()
    {
        if (rb != null)
        {
            isStriking = false;
            isAttacking = true;
            hitBox.SetActive(true);

            Vector2 strikeDirection = ((Vector2)lockedPlayerPos - (Vector2)disturbIndicator.position).normalized;

            UpdateSpriteFacing(strikeDirection);

            rb.linearVelocity = strikeDirection * strikeSpeed;
        }
    }

    IEnumerator ScanPlayer()
    {
        // Scanning: indicator terus ngikutin player real-time (ditangani di Update)
        yield return new WaitForSeconds(scanTime);

        // Lock Player Position — bekukan posisi & rotasi
        lockedPlayerPos = playerPos;
        isLockedOn = true;
        RotateIndicatorTo(lockedPlayerPos); // set rotasi terakhir kali, lalu gak berubah lagi

        yield return new WaitForSeconds(lockTime); // opsional: jeda sebentar sebelum strike, biar ada telegraph

        // Striking
        isStriking = true;
        isScanning = false;
    }

    void UpdateSpriteFacing(Vector2 direction)
    {
        // Hitung sudut arah gerak (0° = kanan, 90° = atas, dst — standar Atan2)
        float rawAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Normalisasi ke rentang -180..180
        rawAngle = Mathf.DeltaAngle(0, rawAngle);

        bool needFlip = rawAngle > 90f || rawAngle < -90f;

        float finalAngle;
        if (needFlip)
        {
            // Cerminkan sudut balik ke rentang -90..90
            // Contoh: 120° -> 60°, -150° -> -30°
            finalAngle = 180f - rawAngle;
            finalAngle = Mathf.DeltaAngle(0, finalAngle);
        }
        else
        {
            finalAngle = rawAngle;
        }

        // Terapkan flip ke scale.x (bukan y, biar gak kebalik atas-bawah)
        Vector3 scale = bodySprite.localScale;
        scale.x = needFlip ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        bodySprite.localScale = scale;

        bodySprite.rotation = Quaternion.Euler(0, 0, finalAngle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Wall"))
        {
            rb.linearVelocity = Vector2.zero;
            isAttacking = false;
            Destroy(gameObject);
        }
    }

    void AnimationHandler()
    {
        airAnim.SetBool("isAttack", isAttacking);
    }
}