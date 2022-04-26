using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float m_health = 20f;

    public float Health { get { return m_health; } }

    public void ReduceHealth(float amount)
    {
        m_health -= amount;
        if (m_health < 0)
        {
            m_health = 0;
        }
    }

}
