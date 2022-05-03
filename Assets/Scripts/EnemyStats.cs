using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private int m_health = 10;
    [SerializeField] private int m_maxHealth = 10;


    public int Health { get { return m_health; } set { m_health = value; } }
    public int MaxHealth { get { return m_maxHealth; } }

    private void Start()
    {
        if (m_health > m_maxHealth)
        {
            m_maxHealth = m_health;
        }
    }
    public void ReduceHealth(int amount)
    {
        m_health -= amount;
        if (m_health < 0)
        {
            m_health = 0;
        }
    }
}
