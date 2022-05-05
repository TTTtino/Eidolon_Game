using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElectricBolt : Projectile
{
    // Does not use gravity and single force on Start
    void Start()
    {
        m_rb2d.gravityScale = 0f;
        m_rb2d.AddForce(transform.up * m_speed, ForceMode2D.Impulse);
    }

    // Interacts with Interactables on contact
    protected override void OnCollide(Collision2D other)
    {
        Switch i = other.gameObject.GetComponent<Switch>();
        if (i != null)
        {
            i.Interact(gameObject);
        }
    }
}
