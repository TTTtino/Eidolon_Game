using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{


    public override void onHit(float damageDealt)
    {
        base.onHit(damageDealt);

        Debug.Log("Bat was dealt " + damageDealt + " damage. Health: " + m_health);

    }

    protected override void onDeath()
    {
        m_animator.SetTrigger("dead");
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(gameObject, m_animator.GetCurrentAnimatorStateInfo(0).length + 2f);
    }
}