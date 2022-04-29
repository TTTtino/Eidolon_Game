using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gate : MonoBehaviour, IInteractable
{
    [SerializeField] bool isOpen = false;
    [SerializeField] UnityEvent m_openEvents;
    [SerializeField] UnityEvent m_closeEvents;

    private void Start()
    {
        if (isOpen)
        {
            OpenGate();
        }
        else
        {
            CloseGate();
        }
    }
    public void Interact(GameObject interactor)
    {

        Toggle();

    }

    public void Open()
    {
        if (isOpen == false)
        {
            Debug.Log("Gate Opening");
            OpenGate();
            m_openEvents.Invoke();

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
