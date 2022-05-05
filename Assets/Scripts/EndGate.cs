using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Calls level complete when player enters
public class EndGate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            LevelController.Instance.LevelComplete();
            other.GetComponent<PlayerController>().m_controllable = false;
        }
    }
}
