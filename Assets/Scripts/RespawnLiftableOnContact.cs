using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Respawns objects with liftable component on contact
public class RespawnLiftableOnContact : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Liftable l = other.gameObject.GetComponent<Liftable>();
        if (l != null)
        {
            l.Respawn();
        }
    }
}
