using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent m_onTriggerEnter;
    [SerializeField] UnityEvent m_onTriggerExit;
    [SerializeField] LayerMask m_triggerLayers;

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((m_triggerLayers.value & (1 << other.transform.gameObject.layer)) > 0) m_onTriggerExit.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((m_triggerLayers.value & (1 << other.transform.gameObject.layer)) > 0) m_onTriggerEnter.Invoke();
    }
}
