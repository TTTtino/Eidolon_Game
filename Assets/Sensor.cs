using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
public class Sensor : MonoBehaviour
{
    [SerializeField] UnityEvent m_onDetected;
    [SerializeField] UnityEvent m_onUndetected;
    [Range(0f, 20f)] public float m_swingSpeed;
    [SerializeField] private bool m_active;
    [SerializeField] private LayerMask m_detectionLayers;
    public bool Active
    {
        get { return m_active; }
        set
        {
            m_active = value;
            BoxCollider2D col = GetComponent<BoxCollider2D>();
            if (col != null)
            {
                if (m_active) col.enabled = true; else col.enabled = false;
            }
        }
    }

    public float m_rotationAngle;
    public float m_detectionAngle;
    public float m_detectionDistance;
    private bool m_detected;
    private List<GameObject> m_detectedObjects;
    private Light2D m_light;
    private float m_time = 0f;
    private PolygonCollider2D m_collider;
    private const int DETECTION_SEGMENTS = 10;

    [SerializeField] Color undetectedColour;
    [SerializeField] Color detectedColour;

    private void Awake()
    {
        m_light = GetComponentInChildren<Light2D>();
        m_detectedObjects = new List<GameObject>();
        m_collider = GetComponent<PolygonCollider2D>();

        float a = Mathf.Deg2Rad * (m_detectionAngle * 2);
        Vector2[] ps = new Vector2[DETECTION_SEGMENTS + 2];
        ps[0] = Vector2.zero;
        for (int i = 0; i < DETECTION_SEGMENTS + 1; i++)
        {
            float t = ((3 * Mathf.PI - a) / 2 + (a / DETECTION_SEGMENTS) * i);
            Vector2 pos = new Vector2(m_detectionDistance * Mathf.Cos(t), m_detectionDistance * Mathf.Sin(t));
            ps[i + 1] = pos;
        }
        m_collider.points = ps;
        m_light.pointLightInnerAngle = m_detectionAngle;
        m_light.pointLightOuterAngle = m_detectionAngle * 2;
        m_light.pointLightOuterRadius = m_detectionDistance;
        m_light.pointLightInnerRadius = m_detectionDistance * 0.6f;

    }


    void Update()
    {
        if (Active)
        {
            m_time += Time.deltaTime * m_swingSpeed;
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, -m_rotationAngle), Quaternion.Euler(0, 0, +m_rotationAngle), (Mathf.Sin(m_time) + 1) / 2);
        }
        // float a = Mathf.Deg2Rad * (m_detectionAngle * 2);
        // float x = (m_detectionDistance * Mathf.Sin(a)) / (Mathf.Sin((Mathf.PI - a) / 2));
        // float y = Mathf.Sqrt(m_detectionDistance * m_detectionDistance + (x * x) / 4);
        // Vector2[] ps = { Vector2.zero, new Vector2(-x / 2, -y), new Vector2(x / 2, -y) };
        // m_collider.points = ps;


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Active)
        {
            m_light.enabled = true;
            if (!m_detectedObjects.Contains(other.gameObject)
            && (m_detectionLayers.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                m_detectedObjects.Add(other.gameObject);
                if (!m_detected)
                {
                    OnDetection();
                    m_detected = true;
                }

            }
        }
        else
        {
            m_light.enabled = false;
        }
    }

    private void OnDetection()
    {
        m_light.color = detectedColour;
        m_onDetected.Invoke();


    }

    private void OnUndetected()
    {
        m_light.color = undetectedColour;
        m_onUndetected.Invoke();

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (Active)
        {
            // Debug.Log(other.name + " leaving detection range");
            if (m_detectedObjects.Contains(other.gameObject))
            {
                m_detectedObjects.Remove(other.gameObject);
            }
            if (m_detectedObjects.Count == 0)
            {
                m_detected = false;
                OnUndetected();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float a = Mathf.Deg2Rad * (m_detectionAngle * 2);
        Vector2[] ps = new Vector2[DETECTION_SEGMENTS + 2];
        ps[0] = Vector2.zero;
        for (int i = 0; i < DETECTION_SEGMENTS + 1; i++)
        {
            float t = ((3 * Mathf.PI - a) / 2 + (a / DETECTION_SEGMENTS) * i) + transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 pos = new Vector2(m_detectionDistance * Mathf.Cos(t), m_detectionDistance * Mathf.Sin(t));
            ps[i + 1] = pos;
            Gizmos.DrawLine(transform.position + (Vector3)ps[i], transform.position + (Vector3)ps[i + 1]);
        }

        Gizmos.DrawLine(transform.position + (Vector3)ps[ps.Length - 1], transform.position + (Vector3)ps[0]);


    }

}
