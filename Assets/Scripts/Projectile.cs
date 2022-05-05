using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all projectles
public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float m_damage = 0.0f;
    [SerializeField] protected float m_speed = 10.0f;
    [SerializeField] protected float m_cooldown = 10.0f;
    [SerializeField] protected bool m_destroyOnCollision = true;

    public bool m_isActive = false;

    protected Rigidbody2D m_rb2d;
    protected virtual void Awake()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        OnCollide(other);
        if (m_destroyOnCollision)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnCollide(Collision2D other)
    {

    }

    public void addSpawnVelocity(Vector2 sVel)
    {
        m_rb2d.velocity += sVel;
    }

    public float GetCooldown()
    {
        return m_cooldown;
    }

}
