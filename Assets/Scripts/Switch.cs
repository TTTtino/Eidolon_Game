using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour, IInteractable
{
    [SerializeField] bool m_isOn = false;
    [SerializeField] UnityEvent m_onSwitchOn;
    [SerializeField] UnityEvent m_onSwitchOff;
    [SerializeField] Sprite m_offSprite;
    [SerializeField] Sprite m_onSprite;


    private void Start()
    {
        if (m_isOn)
        {
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = m_onSprite;
            }
        }
        else
        {
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = m_offSprite;
            }
        }
    }
    public void Interact(GameObject interactor)
    {
        if (m_isOn)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }

    public void TurnOn()
    {
        m_isOn = true;
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = m_onSprite;
        }
        m_onSwitchOn.Invoke();

    }

    public void TurnOff()
    {
        m_isOn = false;
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = m_offSprite;
        }
        m_onSwitchOff.Invoke();
    }

}
