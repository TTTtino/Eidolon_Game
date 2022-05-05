using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Turret that shoots at player when in range and has a direct line of sight
public class Turret : MonoBehaviour
{
    public Transform m_target;
    [SerializeField] GameObject m_turretBarrel;
    [SerializeField] float m_turnSpeed;
    private float m_targetAngle = 0.0f;
    private float m_rotation = 0.0f;
    [SerializeField] private float m_distance = 5;
    public LayerMask m_hitLayers;
    public Transform m_shootPosition;
    // true when direct line of sight with target
    public bool m_targetLockedOn;
    private AudioSource m_source;
    public AudioClip m_shootSound;
    // Ammo that is shot
    [SerializeField] private Projectile m_ammoPrefab;

    private float m_lastAttackTime = -1000f;
    [SerializeField] private float m_attackInterval = 2f;
    public UnityEvent m_onDeath;
    EnemyStats m_stats;
    public bool m_active = true;
    public bool Active { get { return m_active; } set { m_active = value; } }

    // Start is called before the first frame update
    void Start()
    {
        m_source = GetComponent<AudioSource>();
        m_stats = GetComponentInChildren<EnemyStats>();
        Debug.Log(m_stats);
    }

    // Update is called once per frame
    void Update()
    {

        if (!m_active)
        {
            m_rotation = Mathf.Lerp(m_rotation, 0, m_turnSpeed * Time.deltaTime);
            m_turretBarrel.transform.localRotation = Quaternion.Euler(0, 0, -m_rotation);
            if (m_stats.Health < m_stats.MaxHealth)
            {
                m_stats.Health = m_stats.MaxHealth;
            }
            return;
        }
        if (m_stats.Health <= 0 && m_active)
        {
            Die();
        }
        float angle = 0.0f;
        if (m_target != null)
        {
            Vector2 dir = m_turretBarrel.transform.position - m_target.GetComponentInChildren<Collider2D>().bounds.center;
            dir.Normalize();
            angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        }
        else
        {
            if (m_rotation > m_targetAngle - 0.1f && m_rotation < m_targetAngle + 0.1f)
            {
                m_targetAngle = Random.Range(-90f, 90f);
            }
            angle = m_targetAngle;
        }

        m_rotation = Mathf.Lerp(m_rotation, angle, m_turnSpeed * Time.deltaTime);


        m_turretBarrel.transform.localRotation = Quaternion.Euler(0, 0, -m_rotation);

        RaycastHit2D rc = Physics2D.Raycast(m_shootPosition.position, -m_turretBarrel.transform.up, m_distance);
        if (m_target != null && rc.transform != null && (m_hitLayers.value & (1 << rc.transform.gameObject.layer)) > 0)
        {
            m_targetLockedOn = true;
            if (m_lastAttackTime + m_attackInterval < Time.time)
            {
                m_lastAttackTime = Time.time;
                Shoot();
            }
        }
        else
        {
            m_targetLockedOn = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((m_hitLayers.value & (1 << other.transform.gameObject.layer)) > 0 && m_target == null)
        {
            m_target = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (m_target != null && (m_hitLayers.value & (1 << other.transform.gameObject.layer)) > 0 && other.gameObject == m_target.gameObject)
        {
            m_target = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!m_active) return;
        Gizmos.color = m_targetLockedOn ? Color.red : Color.cyan;
        Gizmos.DrawLine(m_shootPosition.position, m_shootPosition.position + (-m_turretBarrel.transform.up * m_distance));
    }

    private void Shoot()
    {
        Projectile p = Instantiate(m_ammoPrefab);
        p.transform.position = m_shootPosition.position;
        p.transform.up = -m_turretBarrel.transform.up;
        m_source.PlayOneShot(m_shootSound);
    }

    void Die()
    {
        m_active = false;
        m_onDeath.Invoke();
    }
}
