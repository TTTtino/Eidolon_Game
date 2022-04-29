using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnContact : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player Collision");
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.Die();
            }
        }
    }
}
