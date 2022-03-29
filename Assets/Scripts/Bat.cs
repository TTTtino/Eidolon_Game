using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Bat : Enemy
{

    [SerializeField] private Vector3 m_targetPosition;
    private Rigidbody2D m_rb2d;
    private Vector2 m_originPosition;
    private Path m_path;
    [SerializeField] private float m_nextWaypointDistance = 3f;
    private int m_currentWaypoint = 0;
    private bool m_reached = false;
    private Seeker m_seeker;
    [SerializeField] private float m_roamingRadius;
    [SerializeField] private float m_maxFollowDistance;
    [SerializeField] private float m_detectionRadius;
    private float lastRoamTime = 0;

    [SerializeField] private GameObject m_damagePoint;

    protected override void Awake()
    {
        base.Awake();
        m_rb2d = GetComponent<Rigidbody2D>();
        m_originPosition = transform.position;
        m_seeker = GetComponent<Seeker>();
    }

    void Start()
    {
    }

    private void OnPathCalculated(Path p)
    {
        if (!p.error)
        {
            m_path = p;
            m_currentWaypoint = 0;
        }
        Debug.Log("Path calculated");
    }

    void Update()
    {
        if (m_rb2d.velocity.x > 0)
        {
            flip(true);
        }
        else
        {
            flip(false);
        }

        switch (m_state)
        {
            case EnemyState.IDLE:

                break;
            case EnemyState.FOLLOWING:
                if (m_targetPlayer != null)
                {
                    if (m_path != null && m_path.GetTotalLength() > m_maxFollowDistance)
                    {
                        m_state = EnemyState.RETURNING;
                    }
                }
                break;
            case EnemyState.RETURNING:
                break;
            default:
                break;
        }
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(m_rb2d.position, m_targetPlayer.transform.position) < m_detectionRadius && m_state != EnemyState.ATTACKING)
        {
            m_state = EnemyState.FOLLOWING;
        }
        switch (m_state)
        {
            case EnemyState.ROAMING:
                if (Vector2.Distance(m_rb2d.position, m_originPosition) <= m_roamingRadius)
                {
                    if (Time.time > lastRoamTime + 3f)
                    {
                        if (m_path == null)
                        {
                            m_targetPosition = getNewTargetPosition();
                            CalculatePath();
                        }
                        else
                        {
                            MoveToTarget();
                            if (m_reached)
                            {
                                lastRoamTime = Time.time;
                                m_path = null;
                            }
                        }
                    }

                }
                else
                {
                    m_state = EnemyState.RETURNING;
                    m_path = null;
                }
                break;
            case EnemyState.FOLLOWING:

                if (m_targetPlayer != null)
                {
                    if (Vector2.Distance(m_rb2d.position, m_targetPlayer.transform.position) < 1f)
                    {
                        Attack();
                    }
                    else
                    {

                        m_targetPosition = m_targetPlayer.transform.position;
                        if (!IsInvoking("CalculatePath"))
                            InvokeRepeating("CalculatePath", 0f, 0.5f);
                        MoveToTarget();
                    }
                }
                break;
            case EnemyState.RETURNING:
                if ((Vector2)m_targetPosition != m_originPosition)
                {
                    m_targetPosition = m_originPosition;
                    if (IsInvoking("CalculatePath"))
                        CancelInvoke("CalculatePath");
                    CalculatePath();
                }
                else
                {
                    MoveToTarget();
                    if (Vector2.Distance(m_rb2d.position, m_originPosition) <= m_roamingRadius)
                    {
                        m_state = EnemyState.ROAMING;
                        m_path = null;
                    }
                }
                break;
            case EnemyState.ATTACKING:
                if (m_targetPlayer.transform.position.x > m_rb2d.position.x)
                {
                    flip(true);
                }
                else
                {
                    flip(false);
                }
                Invoke("CheckHits", m_damage);
                break;
            default:
                break;
        }
    }

    void CalculatePath()
    {
        if (m_seeker.IsDone())
            m_seeker.StartPath(m_rb2d.position, m_targetPosition, OnPathCalculated);

    }
    public override void OnHit(float damageDealt)
    {
        base.OnHit(damageDealt);

        Debug.Log("Bat was dealt " + damageDealt + " damage. Health: " + m_health);

    }


    protected override void OnDeath()
    {
        m_animator.SetTrigger("dead");
        m_rb2d.gravityScale = 1;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(gameObject, m_animator.GetCurrentAnimatorStateInfo(0).length + 2f);
    }

    void MoveToTarget()
    {
        if (m_path == null)
        {
            return;
        }

        if (m_currentWaypoint >= m_path.vectorPath.Count)
        {
            m_reached = true;
            return;
        }
        else
        {
            m_reached = false;
        }

        Vector2 direction = ((Vector2)m_path.vectorPath[m_currentWaypoint] - m_rb2d.position).normalized;
        Vector2 force = direction * m_moveSpeed * Time.deltaTime;

        float dist = Vector2.Distance(m_rb2d.position, m_path.vectorPath[m_currentWaypoint]);

        if (dist < m_nextWaypointDistance)
        {
            m_currentWaypoint++;
        }

        m_rb2d.AddForce(force);
        Debug.Log("Moving");

    }



    protected void onReturning()
    {

    }

    protected void onRoaming()
    {
        // if (Vector2.Distance(m_targetPosition, transform.position) < 0.1f)
        // {
        //     m_targetPosition = getNewTargetPosition();
        //     if (m_targetPosition.x > transform.position.x)
        //     {
        //         flip(true);
        //     }
        //     else
        //     {
        //         flip(false);
        //     }
        // }
        // transform.position = Vector2.MoveTowards(transform.position, m_targetPosition, m_roamSpeed * Time.deltaTime);
    }

    Vector2 getNewTargetPosition()
    {
        Vector2 pos = (Random.insideUnitCircle * m_roamingRadius) + m_originPosition;
        // RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, GetComponent<CircleCollider2D>().radius,
        //     (pos - (Vector2)transform.position), Vector2.Distance(pos, transform.position), m_roamCollision);
        // while (hits.Length > 0)
        // {
        //     pos = (Random.insideUnitCircle * m_roamingRadius) + m_originPosition;
        //     hits = Physics2D.CircleCastAll(transform.position, GetComponent<CircleCollider2D>().radius,
        //         (pos - (Vector2)transform.position), Vector2.Distance(pos, transform.position), m_roamCollision);
        // }
        return pos;
    }

    void flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void flip(bool faceRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        if (!faceRight)
        {
            scale.x *= -1;
        }
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, m_detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_maxFollowDistance);
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(m_originPosition, m_roamingRadius);

        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, m_roamingRadius);

        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_damagePoint.transform.position, m_damageRadius);
    }

    public void CheckHits()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(m_damagePoint.transform.position, m_damageRadius, LayerMask.GetMask("Player"));
        foreach (Collider2D item in hitObjects)
        {
            IHittable hittable = item.GetComponent<IHittable>();
            if (hittable != null && item.tag == "Player")
            {
                m_animator.SetTrigger("attack");
                hittable.OnHit(m_damage);
            }
        }
        m_state = EnemyState.FOLLOWING;
    }

    void Attack()
    {
        if (m_lastAttackTime + m_attackInterval < Time.time)
        {
            m_lastAttackTime = Time.time;
            CheckHits();
        }
    }
}