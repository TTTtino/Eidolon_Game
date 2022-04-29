using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingTrap : MonoBehaviour
{
    [Range(0f, 20f)] public float m_swingSpeed;
    [SerializeField] private bool m_swinging;
    public bool Swinging
    {
        get { return m_swinging; }
        set
        {
            m_swinging = value;
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                if (m_swinging) sr.sprite = m_onSprite; else sr.sprite = m_offSprite;
            }
            BoxCollider2D col = GetComponent<BoxCollider2D>();
            if (col != null)
            {
                if (m_swinging) col.enabled = true; else col.enabled = false;
            }
        }
    }
    public float m_maxAngle;
    private float m_time = 0f;


    [SerializeField] Sprite m_offSprite;
    [SerializeField] Sprite m_onSprite;

    void Update()
    {
        if (Swinging)
        {
            m_time += Time.deltaTime * m_swingSpeed;
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, -m_maxAngle), Quaternion.Euler(0, 0, +m_maxAngle), (Mathf.Sin(m_time) + 1) / 2);

        }
    }

}
