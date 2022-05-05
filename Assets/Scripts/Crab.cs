using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crab : MonoBehaviour
{
    // Used to check if ground is in front
    public Transform m_groundCheck;
    // Used to check if wall is in front
    public Transform m_wallCheck;
    // Events called on death
    public UnityEvent m_onDeath;
    // Radius where enemy can see player (even through walls)
    public float m_detectionRadius = 2f;
    // how much to the left and right from it's origin the crab can move
    public float m_xMoveDist = 2f;
    // movement speed
    public float m_moveSpeed = 1f;

    private Rigidbody2D m_rb2d;
    // maximum left/right position in world
    private float m_maxLeft, m_maxRight;
    // position on start
    private Vector3 m_origin;
    // layers that is checked against for floor and ground
    public LayerMask m_groundLayers;
    // acquired target on trigger enter
    public Transform m_target;
    // layers that can be targeted
    public LayerMask m_canTarget;
    // Move speed when target aquired
    public float m_speedMultiplier = 2f;
    // direction that the crab is moving in
    float xMoveDir = 1;
    EnemyStats m_stats;
    private bool m_dead = false;

    private void Start()
    {
        m_stats = GetComponent<EnemyStats>();
        m_rb2d = GetComponent<Rigidbody2D>();
        m_maxLeft = transform.position.x - m_xMoveDist;
        m_maxRight = transform.position.x + m_xMoveDist;
        m_origin = transform.position;
    }

    private void Update()
    {

        if (m_stats.Health <= 0 && !m_dead)
        {
            Die();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_dead) return;
        Collider2D[] cs =
        Physics2D.OverlapCircleAll(transform.position, m_detectionRadius, m_canTarget);
        m_target = null;
        foreach (Collider2D col in cs)
        {
            if ((m_canTarget.value & (1 << col.transform.gameObject.layer)) > 0)
            {
                m_target = col.transform;
                break;
            }
        }
        // Idle actions
        if (m_target == null)
        {
            if (xMoveDir == 0) xMoveDir = -1;
            if (Physics2D.OverlapCircle(m_wallCheck.position, 0.1f, m_groundLayers))
            {
                Debug.Log("Wall in front");
                xMoveDir *= -1;
            }
            Collider2D c = Physics2D.OverlapCircle(m_groundCheck.position, 0.2f, m_groundLayers);
            if (c == null)
            {
                xMoveDir *= -1;
            }
            Vector3 newScale = transform.localScale;
            if (xMoveDir > 0)
            {
                newScale.x = Mathf.Abs(newScale.x);
                m_rb2d.MovePosition(Vector2.MoveTowards(m_rb2d.position, new Vector2(m_maxRight, m_rb2d.position.y), m_moveSpeed * Time.deltaTime));
            }
            else
            {
                newScale.x = -Mathf.Abs(newScale.x);
                m_rb2d.MovePosition(Vector2.MoveTowards(m_rb2d.position, new Vector2(m_maxLeft, m_rb2d.position.y), m_moveSpeed * Time.deltaTime));
            }

            transform.localScale = newScale;

            if (m_rb2d.position.x > m_maxLeft - 0.1f && m_rb2d.position.x < m_maxLeft + 0.1f)
            {
                xMoveDir = 1;
            }
            else if (m_rb2d.position.x > m_maxRight - 0.1f && m_rb2d.position.x < m_maxRight + 0.1f)
            {
                xMoveDir = -1;
            }
        } // Attacking player actions
        else
        {
            if (Physics2D.OverlapCircle(m_groundCheck.position, 0.2f, m_groundLayers) && Physics2D.OverlapCircle(m_wallCheck.position, 0.1f, m_groundLayers) == null)
            {
                Vector3 newScale = transform.localScale;
                if (m_target.position.x > m_rb2d.position.x)
                {
                    newScale.x = Mathf.Abs(newScale.x);
                }
                else
                {
                    newScale.x = -Mathf.Abs(newScale.x);
                }

                transform.localScale = newScale;
                m_rb2d.MovePosition(Vector2.MoveTowards(m_rb2d.position, new Vector2(m_target.position.x, m_rb2d.position.y), m_moveSpeed * m_speedMultiplier * Time.deltaTime));
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            m_origin = transform.position;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector2(m_origin.x - m_xMoveDist, transform.position.y + 0.5f), new Vector2(m_origin.x + m_xMoveDist, transform.position.y + 0.5f));

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, m_detectionRadius);
    }

    void Die()
    {
        m_dead = true;
        GetComponent<DamagePlayerOnContact>().m_active = false;
        GetComponentInChildren<Animator>().SetTrigger("death");
        Destroy(gameObject, 3f);
        m_onDeath.Invoke();
    }



}
