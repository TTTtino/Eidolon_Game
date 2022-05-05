using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Star collectible in scene
public class Star : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().StarCollected();
            Destroy(gameObject);
        }
    }
}
