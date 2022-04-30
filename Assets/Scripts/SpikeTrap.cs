using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public bool m_activated;
    public AudioClip m_activationSound;
    private AudioSource m_source;
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_animator.SetBool("activated", m_activated);
        m_source = GetComponent<AudioSource>();
    }

    public void SpikeDown()
    {
        if (m_activated)
        {
            m_activated = false;
            m_animator.SetBool("activated", false);
        }
    }

    public void SpikeUp()
    {
        if (!m_activated)
        {
            m_activated = true;
            m_animator.SetBool("activated", true);
            m_source.PlayOneShot(m_activationSound);

        }
    }
}
