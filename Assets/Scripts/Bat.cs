using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    [SerializeField] private float m_roamSpeed;
    private Vector2 m_targetPosition;
    private Vector2 m_originPosition;
    [SerializeField] private float m_roamingRadius;
    [SerializeField] private LayerMask m_roamCollision;

    protected override void Awake()
    {
        base.Awake();
        m_targetPosition = transform.position;
        m_originPosition = transform.position;
        m_roamCollision = LayerMask.GetMask("Environment");
    }

    void Start()
    {

    }
    public override void onHit(float damageDealt)
    {
        base.onHit(damageDealt);

        Debug.Log("Bat was dealt " + damageDealt + " damage. Health: " + m_health);

    }


    protected override void onDeath()
    {
        m_animator.SetTrigger("dead");
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(gameObject, m_animator.GetCurrentAnimatorStateInfo(0).length + 2f);
    }

    protected override void onFollowing()
    {

    }

    protected override void onReturning()
    {

    }

    protected override void onRoaming()
    {
        if (Vector2.Distance(m_targetPosition, transform.position) < 0.1f)
        {
            m_targetPosition = getNewTargetPosition();
            if (m_targetPosition.x > transform.position.x)
            {
                flip(true);
            }
            else
            {
                flip(false);
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, m_targetPosition, m_roamSpeed * Time.deltaTime);
    }

    Vector2 getNewTargetPosition()
    {
        Vector2 pos = (Random.insideUnitCircle * m_roamingRadius) + m_originPosition;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, GetComponent<CircleCollider2D>().radius,
            (pos - (Vector2)transform.position), Vector2.Distance(pos, transform.position), m_roamCollision);
        while (hits.Length > 0)
        {
            pos = (Random.insideUnitCircle * m_roamingRadius) + m_originPosition;
            hits = Physics2D.CircleCastAll(transform.position, GetComponent<CircleCollider2D>().radius,
                (pos - (Vector2)transform.position), Vector2.Distance(pos, transform.position), m_roamCollision);
        }
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

    void OnDrawGizmos()
    {

        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(m_originPosition, m_roamingRadius);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, m_targetPosition);

        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, m_roamingRadius);

        }
    }
}