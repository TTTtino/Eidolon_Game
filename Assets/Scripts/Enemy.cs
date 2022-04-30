using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHittable
{

    protected enum EnemyState { IDLE, ROAMING, FOLLOWING, RETURNING, ATTACKING };
    [SerializeField] protected int m_health = 10;
    [SerializeField] protected float m_moveSpeed = 2;
    [SerializeField] protected int m_damage = 2;
    [SerializeField] protected float m_damageRadius = 2;
    [SerializeField] protected float m_damageDelay = 0.2f;

    protected float m_lastAttackTime = 0f;
    [SerializeField] protected float m_attackInterval = 100f;
    [SerializeField] protected GameObject m_targetPlayer;


    [SerializeField] protected EnemyState m_state = EnemyState.IDLE;
    protected Animator m_animator;

    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public virtual void OnHit(int damageDealt)
    {
        if (m_health > 0)
        {
            m_health -= damageDealt;
            m_animator.SetTrigger("hit");
            if (m_health <= 0)
            {
                m_health = 0;
                OnDeath();
            }
        }
    }

    protected abstract void OnDeath();





}