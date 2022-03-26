using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHittable
{
    [SerializeField] protected float m_health = 100f;

    [SerializeField] protected Animator m_animator;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public virtual void onHit(float damageDealt)
    {
        if (m_health > 0)
        {
            m_health -= damageDealt;
            if (m_health <= 0)
            {
                m_health = 0f;
                onDeath();
            }
        }
    }

    protected abstract void onDeath();
}