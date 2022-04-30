using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    public Transform m_groundCheck;
    public Transform m_wallCheck;
    public float m_detectionRadius = 2f;
    public float m_xMoveDist = 2f;
    public float m_moveSpeed = 1f;
    private Rigidbody2D m_rb2d;
    private float m_maxLeft, m_maxRight;
    private Vector3 m_origin;
    public LayerMask m_groundLayers;
    public Transform m_target;
    public LayerMask m_canTarget;
    // Move speed when target aquired
    public float m_speedMultiplier = 2f;
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
        if (m_target == null)
        {
            if (xMoveDir == 0) xMoveDir = -1;
            if (Physics2D.OverlapCircle(m_wallCheck.position, 0.1f))
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
        }
        else
        {
            if (Physics2D.OverlapCircle(m_groundCheck.position, 0.2f, m_groundLayers) && Physics2D.OverlapCircle(m_wallCheck.position, 0.1f, m_groundLayers) == null)
            {
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
        GetComponentInChildren<Animator>().SetTrigger("death");
        Destroy(gameObject, 3f);
    }
}
