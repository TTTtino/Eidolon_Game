using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic Projectile
public class BasicBolt : Projectile
{
    // Does not use gravity and single force on Start
    void Start()
    {
        m_rb2d.gravityScale = 0f;
        m_rb2d.AddForce(transform.up * m_speed, ForceMode2D.Impulse);
    }
}
