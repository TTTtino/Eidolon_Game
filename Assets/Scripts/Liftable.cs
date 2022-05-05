using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Components used when gameobject can be lifted
public class Liftable : MonoBehaviour
{
    // Sounds to be played on impact (chosen randomly)
    public AudioClip[] m_impactSounds;
    // sprite that renders
    SpriteRenderer m_renderer;
    // audio source to play sounds through
    private AudioSource m_source;
    // Player holding the object
    PlayerController m_holder;
    Rigidbody2D m_rb2d;
    // initial postition of the object
    private Vector3 m_spawnPos;
    // true if should be respawned
    public bool m_respawn;

    private void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_source = GetComponent<AudioSource>();
        m_spawnPos = transform.position;
        m_renderer = GetComponent<SpriteRenderer>();
    }
    public void DropItem()
    {
        Debug.Log("Dropping Item");
        m_holder = null;
        m_rb2d.simulated = true;
        ChangeTransparency(1.0f);
    }

    public void PickUpItem(GameObject picker)
    {
        Debug.Log("Picked up " + gameObject.name);
        m_holder = picker.GetComponent<PlayerController>();
        m_rb2d.simulated = false;
        ChangeTransparency(0.6f);

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

    public void Respawn()
    {
        if (m_respawn)
        {
            transform.position = m_spawnPos;
            transform.rotation = Quaternion.identity;
            m_rb2d.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (m_source != null && m_impactSounds.Length > 0 && other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            m_source.PlayOneShot(m_impactSounds[Random.Range(0, m_impactSounds.Length)]);
        }
    }

    private void ChangeTransparency(float transparency)
    {
        transparency = Mathf.Clamp(transparency, 0, 1);
        Color newCol = m_renderer.color;
        newCol.a = transparency;
        m_renderer.color = newCol;
    }
}
