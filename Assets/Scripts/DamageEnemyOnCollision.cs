using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemyOnCollision : MonoBehaviour
{
    public int m_damageAmount;
    private float m_lastDamageTime = -1000f;
    public float m_damageInterval = 1f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyStats es = other.gameObject.GetComponent<EnemyStats>();
            if (es != null)
            {
                m_lastDamageTime = Time.time;
                es.ReduceHealth(m_damageAmount);
            }
        }
    }

    // Update is called once per frame
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyStats es = other.gameObject.GetComponent<EnemyStats>();
            if (es != null)
            {
                if (m_lastDamageTime + m_damageInterval < Time.time)
                {
                    m_lastDamageTime = Time.time;
                    es.ReduceHealth(m_damageAmount);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_lastDamageTime = -1000f;
        }
    }
}
