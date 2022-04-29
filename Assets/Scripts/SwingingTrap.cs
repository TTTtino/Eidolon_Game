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

    private void OnDrawGizmos()
    {
        DrawWireArc(transform.position, Vector3.down, m_maxAngle * 2, 1.5f * transform.localScale.magnitude);
    }

    public static void DrawWireArc(Vector3 position, Vector3 dir, float anglesRange, float radius, float maxSteps = 20)
    {
        var srcAngles = GetAnglesFromDir(position, dir);
        var initialPos = position;
        var posA = initialPos;
        var stepAngles = anglesRange / maxSteps;
        var angle = srcAngles - anglesRange / 2;
        for (var i = 0; i <= maxSteps; i++)
        {
            var rad = Mathf.Deg2Rad * angle;
            var posB = initialPos;
            // posB += new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));
            posB += new Vector3(radius * Mathf.Cos(rad), radius * Mathf.Sin(rad), 0);


            Gizmos.DrawLine(posA, posB);

            angle += stepAngles;
            posA = posB;
        }
        Gizmos.DrawLine(posA, initialPos);
    }

    static float GetAnglesFromDir(Vector3 position, Vector3 dir)
    {
        var forwardLimitPos = position + dir;
        // var srcAngles = Mathf.Rad2Deg * Mathf.Atan2(forwardLimitPos.z - position.z, forwardLimitPos.x - position.x);
        var srcAngles = Mathf.Rad2Deg * Mathf.Atan2(forwardLimitPos.y - position.y, forwardLimitPos.x - position.x);

        return srcAngles;
    }

}
