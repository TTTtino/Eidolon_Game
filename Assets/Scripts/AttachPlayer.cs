using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attaches the player to an object on entering, and whilst the player is not providing a control direction
public class AttachPlayer : MonoBehaviour
{
    PlayerController pControl;
    Vector2 m_offset;
    bool m_playerOnObject;

    // Start is called before the first frame update
    void Start()
    {
        pControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(pControl.ControlDirection);
        if (m_playerOnObject)
        {
            if (pControl.ControlDirection == Vector2.zero)
            {
                pControl.transform.position = transform.position - (Vector3)m_offset;
            }
            else
            {
                m_offset = transform.position - pControl.transform.position;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_playerOnObject = true;
            m_offset = transform.position - other.transform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_playerOnObject = false;
        }
    }
}
