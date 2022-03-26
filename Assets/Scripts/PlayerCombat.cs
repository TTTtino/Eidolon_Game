using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator m_animator;
    private float m_lastAttackTime = 0f;
    [SerializeField] private float m_attackInterval = 100f;
    [SerializeField] private GameObject m_damagePoint;
    [SerializeField] private float m_damageRadius = 0.5f;
    [SerializeField] private LayerMask m_hittableLayers;
    private bool m_mustAttack = false;

    void Awake()
    {

        m_animator = GetComponent<Animator>();

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


    public void onAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_mustAttack = true;
        }
    }

    void OnDrawGizmos()
    {
        if (m_damagePoint.activeSelf)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_damagePoint.transform.position, m_damageRadius);
        }
    }

    // Called by animation event
    public void checkHits()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(m_damagePoint.transform.position, m_damageRadius, m_hittableLayers);
        foreach (Collider2D item in hitObjects)
        {
            IHittable hittable = item.GetComponent<IHittable>();
            if (hittable != null)
            {
                hittable.onHit(2f);
            }
        }
    }
}
