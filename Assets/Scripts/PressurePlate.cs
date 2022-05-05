using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public AudioClip m_sound;
    private AudioSource m_source;
    // Event triggered when player presses the pressure plate
    [SerializeField] UnityEvent m_onEnter;
    // Event triggered when player leaves the pressure plate
    [SerializeField] UnityEvent m_onExit;
    public bool m_pressed;
    // minimum rigidbody weight required to press the pressure plate
    public float m_minWeight = 1f;
    // true if plate should not go up on leaving the plate
    public bool m_keepPressed;
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D oRb2d = other.gameObject.GetComponent<Rigidbody2D>();
        if (oRb2d != null && oRb2d.bodyType == RigidbodyType2D.Dynamic && oRb2d.mass >= m_minWeight)
        {
            Debug.Log(other.name + " entered pressure plate");
            if (m_pressed == false)
            {
                PlateDown();
            }
            else if (m_keepPressed && m_pressed == true)
            {
                PlateUp();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        Rigidbody2D oRb2d = other.gameObject.GetComponent<Rigidbody2D>();
        if (oRb2d != null && oRb2d.bodyType == RigidbodyType2D.Dynamic && oRb2d.mass >= m_minWeight)
        {
            if (m_pressed == false)
            {
                PlateDown();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D oRb2d = other.gameObject.GetComponent<Rigidbody2D>();
        if (oRb2d != null && oRb2d.bodyType == RigidbodyType2D.Dynamic && oRb2d.mass >= m_minWeight)
        {
            Debug.Log(other.name + " exited pressure plate");
            if (m_pressed == true && !m_keepPressed)
            {
                PlateUp();
            }
        }
    }

    private void PlateDown()
    {
        if (!m_pressed)
        {
            m_pressed = true;
            m_animator.SetBool("pressed", true);
            m_source.PlayOneShot(m_sound);
            m_onEnter.Invoke();

        }
    }

    private void PlateUp()
    {
        if (m_pressed)
        {
            m_pressed = false;
            m_animator.SetBool("pressed", false);
            m_source.PlayOneShot(m_sound);
            m_onExit.Invoke();

        }
    }


}
