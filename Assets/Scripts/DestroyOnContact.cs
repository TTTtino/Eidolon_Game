using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    public float m_destroyDelay = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        DestroyAfterDelay();
    }

    // plays animation and then destroys itself
    void DestroyAfterDelay()
    {
        GetComponent<Animator>().SetTrigger("crumble");
        Destroy(gameObject, m_destroyDelay);
    }
}
