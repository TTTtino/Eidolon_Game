using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liftable : MonoBehaviour
{
    PlayerController m_holder;
    Rigidbody2D m_rb2d;

    private void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
    }
    public void DropItem()
    {
        Debug.Log("Dropping Item");
        m_holder = null;
        m_rb2d.simulated = true;
    }

    public void PickUpItem(GameObject picker)
    {
        Debug.Log("Picked up " + gameObject.name);
        m_holder = picker.GetComponent<PlayerController>();
        m_rb2d.simulated = false;
    }

    void Update()
    {
        if (m_holder != null)
        {
            transform.position = m_holder.m_holdPosition.position;
        }
    }

    private void OnDestroy()
    {
        if (m_holder != null)
        {
            m_holder.DropHeldItem();

        }
    }
}
