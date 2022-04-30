using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int m_health = 10;
    [SerializeField] private int m_maxHealth = 10;

    private int m_damage = 1;
    public float Health { get { return m_health; } }
    public float MaxHealth { get { return m_maxHealth; } }
    public float Damage { get { return m_damage; } }

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
