using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBolt : Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        m_rb2d.gravityScale = 0f;
        m_rb2d.AddForce(transform.up * m_speed, ForceMode2D.Impulse);
    }

    protected override void OnCollide(Collision2D other)
    {
        Switch i = other.gameObject.GetComponent<Switch>();
        if (i != null)
        {
            i.Interact(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
