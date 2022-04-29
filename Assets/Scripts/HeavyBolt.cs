using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBolt : Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        m_rb2d.gravityScale = 1f;

        m_rb2d.AddForce(transform.up * m_speed, ForceMode2D.Impulse);
    }

}
