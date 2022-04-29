using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public bool m_activated;
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_animator.SetBool("activated", m_activated);
    }

    public void SpikeDown()
    {
        m_activated = false;
        m_animator.SetBool("activated", false);
    }

    public void SpikeUp()
    {
        m_activated = true;
        m_animator.SetBool("activated", true);
    }
}
