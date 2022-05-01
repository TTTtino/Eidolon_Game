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

    private void OpenGate()
    {
        isOpen = true;
        GetComponentInChildren<Animator>().SetBool("open", true);
    }

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

    private void CloseGate()
    {
        isOpen = false;
        GetComponentInChildren<Animator>().SetBool("open", false);
    }

    public void Toggle()
    {
        if (isOpen) Close(); else Open();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
