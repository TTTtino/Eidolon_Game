using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Destroys projectile that enters the collider
public class DestroyProjectileOnContact : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Projectile l = other.gameObject.GetComponent<Projectile>();
        if (l != null)
        {
            Destroy(l.gameObject);
        }
    }
}
