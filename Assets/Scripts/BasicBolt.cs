using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBolt : Projectile
{
    void Start()
    {
        m_rb2d.gravityScale = 0f;
        m_rb2d.AddForce(transform.up * m_speed, ForceMode2D.Impulse);
    }
}
