using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float m_damage = 0.0f;
    public Rigidbody2D rb2d;
    protected void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        Move();
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }

    protected virtual void Move()
    {
        rb2d.AddForce(transform.up * 10f, ForceMode2D.Impulse);
    }
}
