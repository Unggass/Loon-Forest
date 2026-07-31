using System.Collections;
using UnityEngine;

public class DisturberGround : MonoBehaviour
{
    #region Dusturber Data
    [Header("Target")]
    [SerializeField] Transform player;
    [Space(10)]

    [SerializeField] float strikeSpeed = 5f;
    [SerializeField] float strikeDistance = 3f;
    [Space(5)]
    [SerializeField] float scanDuration = 3f;
    [SerializeField] float minLockDuration = 1.5f;
    [SerializeField] float maxLockDuration = 3f;

    [Space(20)]
    [SerializeField] CapsuleCollider2D bodyCollider;
    [SerializeField] GameObject hitBox;
    [SerializeField] Animator groundAnim;
    #endregion

    float playerPos;
    float strikeDirection;
    bool isStriking = false;
    bool hasScanned = false;

    Vector2 startPos;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        playerPos = player.position.x - rb.transform.position.x;

        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Arena")) && !hasScanned)
        {
            hasScanned = true;
            StartCoroutine(ScanningPlayer());
        }

        if (isStriking)
        {
            StrikeForward(strikeDirection);
        }

        Animationhandler();
    }

    void StrikeForward(float direction)
    {
        float travelDistance = Mathf.Abs(transform.position.x - startPos.x);

        if (travelDistance >= strikeDistance || bodyCollider.IsTouchingLayers(LayerMask.GetMask("Wall")))
        {
            rb.linearVelocity = Vector2.zero;
            isStriking = false;
            Destroy(gameObject);
        }
        else
        {
            hitBox.SetActive(true);
            rb.linearVelocity = new Vector2(strikeSpeed * direction, rb.linearVelocity.y);
        }
    }

    IEnumerator ScanningPlayer()
    {
        float timer = 0f;
        float lockDuration = Random.Range(minLockDuration,maxLockDuration);

        while (timer < scanDuration)
        {
            strikeDirection = Mathf.Sign(playerPos);
            transform.localScale = new Vector3(strikeDirection, transform.localScale.y, transform.localScale.z);

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(lockDuration);

        startPos = transform.position;
        isStriking = true;
    }

    void Animationhandler()
    {
        groundAnim.SetBool("isAttack", isStriking);
    }
}