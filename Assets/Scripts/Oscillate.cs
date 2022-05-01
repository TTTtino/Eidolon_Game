using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{

    [SerializeField] Vector2 m_pos;
    [SerializeField] private bool m_moving;
    [SerializeField] private float m_moveSpeed;
    private Vector2 m_origin;

    private float m_time = 0.0f;
    public bool Moving
    {
        get { return m_moving; }
        set { m_moving = value; }
    }

    private void Awake()
    {
        m_origin = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        if (Moving)
        {
            m_time += Time.deltaTime * m_moveSpeed;
            transform.position = Vector2.Lerp(m_origin, m_origin + m_pos, (Mathf.Sin(m_time) + 1) / 2);
        }
    }

    private void OnDrawGizmos()
    {
        if (m_moving && enabled)
        {
            Gizmos.color = Color.cyan;
            if (Application.isPlaying)
            {
                Gizmos.DrawLine(m_origin, m_origin + m_pos);
            }
            else
            {
                Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + m_pos);
            }
        }
    }
}
