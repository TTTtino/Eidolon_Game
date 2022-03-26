using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private Vector2 m_controlDirection = Vector2.zero;
    private Rigidbody2D m_rb2d;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;
    private bool m_mustJump = false;
    [SerializeField] private bool m_isGrounded = false;
    private bool m_isLeft;
    private Vector3 m_velocity = Vector3.zero;

    void Awake()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

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
        m_isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_groundCheck.position, m_groundCheckRadius, m_groundLayers);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_isGrounded = true;
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
            if (m_isGrounded)
            {
                m_isGrounded = false;

                Vector2 v = m_rb2d.velocity;
                v.y = 0;
                m_rb2d.velocity = v;
                m_rb2d.AddForce(new Vector2(0f, m_jumpForce));
            }
        }

    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_mustJump = true;
        }
    }

    public void onMove(InputAction.CallbackContext context)
    {
        m_controlDirection = context.ReadValue<Vector2>();
        if (context.started)
        {

            m_isLeft = (m_controlDirection.x < 0) ? true : false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(m_groundCheck.position, m_groundCheckRadius);
    }


}
