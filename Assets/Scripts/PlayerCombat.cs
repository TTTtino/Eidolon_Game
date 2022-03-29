using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour, IHittable
{
    // Start is called before the first frame update
    private PlayerStats m_stats;
    private Animator m_animator;
    private float m_lastAttackTime = 0f;
    [SerializeField] private float m_attackInterval = 100f;
    [SerializeField] private GameObject m_damagePoint;
    [SerializeField] private float m_damageRadius = 0.5f;
    [SerializeField] private float m_damageDelay = 0.5f;
    [SerializeField] private LayerMask m_hittableLayers;
    private bool m_mustAttack = false;

    void Awake()
    {
        m_stats = GetComponent<PlayerStats>();
        m_animator = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (m_lastAttackTime + m_attackInterval < Time.time && m_mustAttack)
        {
            m_lastAttackTime = Time.time;
            m_animator.SetTrigger("attack");
        }

        m_mustAttack = false;
    }


    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (m_lastAttackTime + m_attackInterval < Time.time)
            {
                m_lastAttackTime = Time.time;
                m_animator.SetTrigger("attack");
                Invoke("CheckHits", m_damageDelay);
            }
        }
    }

    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_damagePoint.transform.position, m_damageRadius);
    }

    // Called by animation event
    public void CheckHits()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(m_damagePoint.transform.position, m_damageRadius, m_hittableLayers);
        foreach (Collider2D item in hitObjects)
        {
            IHittable hittable = item.GetComponent<IHittable>();
            if (hittable != null)
            {
                hittable.OnHit(2f);
            }
        }
    }

    public void OnHit(float damageDealt)
    {
        m_stats.ReduceHealth(damageDealt);
        if (m_stats.Health <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
