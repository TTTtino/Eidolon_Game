using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLaser : Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        m_rb2d.gravityScale = 0f;
        m_rb2d.AddForce(transform.up * m_speed, ForceMode2D.Impulse);
        Debug.Log("Shots fired");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
