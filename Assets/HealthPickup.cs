using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour, IInteractor
{
    private int m_healthRecovered = 1;
    public void Use()
    {
        PlayerStats s = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        s.Health += m_healthRecovered;
        Destroy(gameObject);

    }


}
