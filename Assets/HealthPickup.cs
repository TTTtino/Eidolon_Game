using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private int m_healthRecovered = 1;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerStats s = other.gameObject.GetComponent<PlayerStats>();
            s.Health += m_healthRecovered;
            Destroy(gameObject);

        }
    }


}
