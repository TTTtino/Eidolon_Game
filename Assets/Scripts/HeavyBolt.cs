using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// not used in game (did not fit into game when started level design)
public class HeavyBolt : Projectile
{
    // Uses gravity and single force on Start
    void Start()
    {
        m_rb2d.gravityScale = 1f;

        m_rb2d.AddForce(transform.up * m_speed, ForceMode2D.Impulse);
    }

}
