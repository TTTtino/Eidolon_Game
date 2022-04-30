using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndWait : MonoBehaviour
{

    [SerializeField] Vector2 m_pos;
    [SerializeField] private bool m_moving;
    [SerializeField] private float m_moveSpeed;
    private Vector2 m_origin;
    private Vector2 m_targetPos;
    [SerializeField] private float m_waitTime;
    private bool m_waiting;


    public bool Moving
    {
        get { return m_moving; }
        set { m_moving = value; }
    }

    private void Awake()
    {
        m_origin = transform.position;
        m_targetPos = m_origin + m_pos;
    }


    // Update is called once per frame
    void Update()
    {
        if (Moving && !m_waiting)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_targetPos, m_moveSpeed * Time.deltaTime);
            if ((Vector2)transform.position == m_origin)
            {
                m_targetPos = m_origin + m_pos;
                StartCoroutine(StartWaiting());
            }
            else if ((Vector2)transform.position == m_origin + m_pos)
            {
                m_targetPos = m_origin;
                StartCoroutine(StartWaiting());
            }
        }
    }

    IEnumerator StartWaiting()
    {
        m_waiting = true;

        yield return new WaitForSeconds(m_waitTime);

        m_waiting = false;
    }

    private void OnDrawGizmos()
    {
        if (m_moving && enabled)
        {
            Gizmos.color = Color.green;
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
