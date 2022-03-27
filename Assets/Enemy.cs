using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHittable
{

    protected enum EnemyState { IDLE, ROAMING, FOLLOWING, RETURNING };
    [SerializeField] protected float m_health = 100f;
    [SerializeField] protected bool m_boundFollow = false;
    [SerializeField] protected float m_boundFollowRadius = 20f;


    [SerializeField] protected EnemyState m_state = EnemyState.IDLE;
    protected Animator m_animator;

    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch (m_state)
        {
            case EnemyState.IDLE:
                onIdle();
                break;
            case EnemyState.ROAMING:
                onRoaming();
                break;
            case EnemyState.FOLLOWING:
                onFollowing();
                break;
            case EnemyState.RETURNING:
                onReturning();
                break;
            default:
                break;
        }
    }

    public virtual void onHit(float damageDealt)
    {
        if (m_health > 0)
        {
            m_health -= damageDealt;
            m_animator.SetTrigger("hit");
            if (m_health <= 0)
            {
                m_health = 0f;
                onDeath();
            }
        }
    }

    protected abstract void onDeath();
    protected virtual void onIdle()
    {

    }
    protected abstract void onRoaming();
    protected abstract void onFollowing();
    protected abstract void onReturning();


}