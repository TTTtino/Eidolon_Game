using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int m_health = 10;
    public int m_maxHealth = 10;

    public float Health { get { return m_health; } }
    public float MaxHealth { get { return m_maxHealth; } }

    private void Start()
    {
        if (m_maxHealth > 20)
        {
            m_maxHealth = 20;
        }
        if (m_health > 20)
        {
            m_health = 20;
        }
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
