using UnityEngine;

// 플랫포머 캐릭터 컨트롤 스크립트
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private int jumpCount = 0;
    private int maxJumpCount = 3; // 점프 가능 횟수

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // X축 움직임과 회전을 고정
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        jumpCount++;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
}
