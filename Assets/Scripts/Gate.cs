using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;


public class Gate : MonoBehaviour, IInteractable
{
    public AudioClip m_sound;
    private AudioSource m_source;
    [SerializeField] bool isOpen = false;
    private Light2D m_light;
    [SerializeField] UnityEvent m_openEvents;
    [SerializeField] UnityEvent m_closeEvents;

    private void Start()
    {

        m_light = GetComponentInChildren<Light2D>();
        if (isOpen)
        {
            OpenGate();
            m_light.enabled = false;
        }
        else
        {
            CloseGate();
            m_light.enabled = true;
        }

        m_source = GetComponent<AudioSource>();
    }
    public void Interact(GameObject interactor)
    {

        Toggle();

    }

    // Opens gate, disables light and plays sound
    public void Open()
    {
        if (!isOpen)
        {
            Debug.Log("Gate Opening");
            OpenGate();
            m_openEvents.Invoke();
            m_light.enabled = false;

            m_source.PlayOneShot(m_sound);

        }
    }

    // activates the gate open animation
    private void OpenGate()
    {
        isOpen = true;
        GetComponentInChildren<Animator>().SetBool("open", true);
    }

    // Opens gate, enables light and plays sound
    public void Close()
    {
        if (isOpen == true)
        {
            Debug.Log("Gate Opening");
            CloseGate();
            m_closeEvents.Invoke();
            m_light.enabled = true;

            m_source.PlayOneShot(m_sound);
        }
    }

    // activates the gate close animation
    private void CloseGate()
    {
        isOpen = false;
        GetComponentInChildren<Animator>().SetBool("open", false);
    }

    // Toggles the gate
    public void Toggle()
    {
        if (isOpen) Close(); else Open();
    }



}
