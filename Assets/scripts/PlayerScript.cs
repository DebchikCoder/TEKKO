using Unity.Cinemachine;
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

    [Header("Spell stuff")]
    [SerializeField] private float castRadius = 10.5f;
    [SerializeField] private float throwForce = 5f;
    [SerializeField] private GameObject bullet;

    [SerializeField] private Transform test;

    private Vector3 m_mousePos;
    private Vector3 m_mouseScrPos;

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

        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
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

    void Shoot()
    {
        m_mousePos = Input.mousePosition;
        m_mouseScrPos = Camera.main.ScreenToWorldPoint(m_mousePos);
        m_mouseScrPos.z = 0;

        Vector2 vel;

        Vector2 lookDir = ((Vector2)m_mouseScrPos - (Vector2)transform.position).normalized;
        Vector2 spawnPosition = (Vector2)transform.position + lookDir * castRadius;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion bulletAngle = Quaternion.Euler(0.0f, 0.0f, angle);

        GameObject Projectile = Instantiate(bullet, spawnPosition, bulletAngle);

        if(m_rb.linearVelocity != Vector2.zero)
        {
            vel = m_rb.linearVelocity.Abs();
        }
        else
        {
            vel = new Vector2(1, 1);
        }

        Rigidbody2D projectileBody = Projectile.GetComponent<Rigidbody2D>();
        if(projectileBody != null)
        {
            projectileBody.AddForce(lookDir * vel * throwForce, ForceMode2D.Impulse);
        }
    }
}
