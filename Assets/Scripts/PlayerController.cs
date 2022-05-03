using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_airMoveSpeed;
    [SerializeField] private float m_moveSmoothTime = 0.05f;
    [SerializeField] private float m_airMoveSmoothTime = 0.4f;
    [SerializeField] private float m_jumpForce;
    [SerializeField] private LayerMask m_groundLayers;
    [SerializeField] private Transform m_groundCheck;
    [SerializeField] private float m_groundCheckRadius;
    private int m_starsCollected = 0;
    public int StarsCollected { get { return m_starsCollected; } }
    private Vector2 m_controlDirection = Vector2.zero;
    private PlayerStats m_stats;
    public Vector2 ControlDirection { get { return m_controlDirection; } }
    public Rigidbody2D m_rb2d;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;
    private bool m_mustJump = false;
    public bool m_controllable = true;
    [SerializeField] private int m_jumpCount = 0;
    public int m_maxJump = 2;
    [SerializeField] private bool m_isGrounded = false;
    public bool Grounded { get { return m_isGrounded; } }
    public bool m_isLeft;
    private Vector3 m_velocity = Vector3.zero;
    private Liftable m_heldItem;
    public Transform m_holdPosition;
    public GameObject m_onDeathCanvas;

    void Awake()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_animator = GetComponentInChildren<Animator>();
        m_stats = GetComponent<PlayerStats>();
        m_starsCollected = 0;
        m_controllable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_controllable) return;
        float xDelta = Mouse.current.position.ReadValue().x - Camera.main.WorldToScreenPoint(transform.position).x;
        if (m_controlDirection != Vector2.zero && m_isGrounded)
        {
            m_isLeft = (m_controlDirection.x < 0) ? true : false;
        }
        else
        {
            m_isLeft = (xDelta < 0) ? true : false;
        }

        if (m_isLeft)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        m_animator.SetFloat("moveSpeed", Mathf.Abs(m_controlDirection.x));
        m_animator.SetBool("grounded", m_isGrounded);

    }

    void FixedUpdate()
    {
        if (!m_controllable) return;
        m_isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_groundCheck.position, m_groundCheckRadius, m_groundLayers);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && !m_isGrounded)
            {
                m_isGrounded = true;
                m_jumpCount = 0;
                break;
            }
        }

        float speedMult = m_moveSpeed;
        float smoothTime = m_moveSmoothTime;
        if (!m_isGrounded)
        {
            speedMult = m_airMoveSpeed;
            smoothTime = m_airMoveSmoothTime;
        }
        Vector3 moveVelocity = new Vector2(m_controlDirection.x * speedMult * Time.fixedDeltaTime, m_rb2d.velocity.y);
        m_rb2d.velocity = Vector3.SmoothDamp(m_rb2d.velocity, moveVelocity, ref m_velocity, smoothTime);

        if (m_mustJump)
        {
            m_mustJump = false;
            m_controlDirection.y = 0;
            if (m_isGrounded || m_jumpCount < m_maxJump)
            {
                // Debug.Log("Jump Count = " + m_jumpCount + ", m_maxJump = " + m_maxJump + ", isGrounded = " + m_isGrounded);
                m_isGrounded = false;
                m_jumpCount++;
                Vector2 v = m_rb2d.velocity;
                v.y = 0;
                m_rb2d.velocity = v;
                m_rb2d.AddForce(new Vector2(0f, m_jumpForce));
            }
        }
        if (m_stats.Health <= 0)
        {
            Die();
        }

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && m_controllable)
        {
            m_mustJump = true;
            m_controlDirection.y = 1;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_controlDirection = context.ReadValue<Vector2>();
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (context.started && m_controllable)
        {
            if (m_heldItem == null)
            {
                Liftable nearestItem = GetNearestLiftable();
                if (nearestItem != null)
                {
                    nearestItem.PickUpItem(gameObject);

                    m_maxJump = 1;
                    m_heldItem = nearestItem;
                }

            }
            else
            {
                DropHeldItem();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(m_groundCheck.position, m_groundCheckRadius);
    }

    public Vector2 GetRBVelocity()
    {
        return m_rb2d.velocity;
    }

    private Liftable GetNearestLiftable()
    {
        float nearestDist = float.MaxValue;
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, 2.0f);
        Liftable item = null;
        foreach (Collider2D hitCol in nearbyColliders)
        {
            Liftable i = hitCol.GetComponent<Liftable>();
            float dist = Vector2.Distance(transform.position, hitCol.transform.position);
            if (i != null && dist < nearestDist)
            {
                item = i;
                nearestDist = dist;
            }
        }
        return item;
    }

    public void UseHeldItem(InputAction.CallbackContext context)
    {
        if (context.started && m_heldItem != null && m_controllable)
        {
            // Check if Interactor
            IInteractor i = m_heldItem.gameObject.GetComponent<IInteractor>();
            if (i != null)
            {
                i.Use();
            }
        }
    }

    public void DropHeldItem()
    {
        if (m_heldItem != null)
        {
            m_heldItem.DropItem();
            Rigidbody2D rb2d = m_heldItem.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                rb2d.velocity = m_rb2d.velocity;
            }
            m_heldItem = null;
            m_maxJump = 2;
        }
    }

    public void TeleportToPosition(Vector3 position)
    {
        transform.position = position;
        m_rb2d.velocity = Vector2.zero;
    }

    public void Die()
    {
        ShowOnDeathScreen();
        m_stats.Health = 0;
        m_controlDirection = Vector2.zero;
        m_controllable = false;
        DropHeldItem();
        m_animator.SetTrigger("death");
        m_rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void ShowOnDeathScreen()
    {
        m_onDeathCanvas.SetActive(true);
    }

    public void StarCollected()
    {
        m_starsCollected++;
    }


}
