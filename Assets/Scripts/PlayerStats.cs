using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int m_health = 10;
    [SerializeField] private int m_maxHealth = 10;

    private int m_damage = 1;
    public int Health
    {
        get { return m_health; }
        set
        {
            m_health = value;
            if (m_health > m_maxHealth)
            {
                m_health = m_maxHealth;
            }
        }
    }
    public int MaxHealth { get { return m_maxHealth; } set { m_maxHealth = value; } }
    public int Damage { get { return m_damage; } }

    private void Start()
    {
        m_health = m_maxHealth;
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
