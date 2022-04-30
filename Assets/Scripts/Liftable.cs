using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liftable : MonoBehaviour
{
    public AudioClip[] m_impactSounds;
    private AudioSource m_source;
    PlayerController m_holder;
    Rigidbody2D m_rb2d;

    private void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_source = GetComponent<AudioSource>();
    }
    public void DropItem()
    {
        Debug.Log("Dropping Item");
        m_holder = null;
        m_rb2d.simulated = true;
    }

    public void PickUpItem(GameObject picker)
    {
        Debug.Log("Picked up " + gameObject.name);
        m_holder = picker.GetComponent<PlayerController>();
        m_rb2d.simulated = false;
    }

    void Update()
    {
        if (m_holder != null)
        {
            transform.position = m_holder.m_holdPosition.position;
        }
    }

    private void OnDestroy()
    {
        if (m_holder != null)
        {
            m_holder.DropHeldItem();

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (m_source != null && m_impactSounds.Length > 0 && other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            m_source.PlayOneShot(m_impactSounds[Random.Range(0, m_impactSounds.Length)]);
        }
    }
}
