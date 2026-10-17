using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D m_rb;
    [Header("Ground parametres")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.45f;
    [SerializeField] private LayerMask ground;
    private bool isGrounded;

    [Header("Physics")]
    [SerializeField] private float MaxSpeed = 12f;
    [SerializeField] private float accel = 90f;
    [SerializeField] private float deccel = 15f;
    [SerializeField] private float jumpForce = 10f;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, ground);
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        float input = Input.GetAxisRaw("Horizontal");
        float targetDir = input * MaxSpeed;
        float rate = input != 0 ? accel : (isGrounded ? deccel : 0f);

        m_rb.linearVelocity = new Vector2(Mathf.MoveTowards(m_rb.linearVelocity.x, targetDir, rate * Time.fixedDeltaTime), m_rb.linearVelocityY);
    }

    void Jump()
    {
        m_rb.linearVelocity = new Vector2(m_rb.linearVelocity.x, 0f);
        m_rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
