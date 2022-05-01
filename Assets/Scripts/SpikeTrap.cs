using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class SpikeTrap : MonoBehaviour
{
    public bool m_activated;
    public AudioClip m_activationSound;
    private AudioSource m_source;
    private Animator m_animator;
    private Light2D m_light;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_animator.SetBool("activated", m_activated);
        m_source = GetComponent<AudioSource>();
        m_light = GetComponentInChildren<Light2D>();
        if (m_activated)
        {
            m_light.enabled = true;
        }
        else
        {
            m_light.enabled = false;
        }
    }

    public void SpikeDown()
    {
        if (m_activated)
        {
            m_activated = false;
            m_light.enabled = false;
            m_animator.SetBool("activated", false);
        }
    }

    public void SpikeUp()
    {
        if (!m_activated)
        {
            m_activated = true;
            m_light.enabled = true;
            m_animator.SetBool("activated", true);
            m_source.PlayOneShot(m_activationSound);

        }
    }
}
