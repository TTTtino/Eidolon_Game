using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent m_onTriggerEnter;
    [SerializeField] UnityEvent m_onTriggerExit;

    private void OnTriggerExit2D(Collider2D other)
    {
        m_onTriggerExit.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        m_onTriggerEnter.Invoke();
    }
}
