using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBolt : Projectile
{
    PlayerController m_attachedPlayer;

    [SerializeField][Range(0.01f, 0.2f)] float m_stopThreshold;
    protected override void Awake()
    {
        base.Awake();
        m_attachedPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_rb2d.gravityScale = 1f;
        m_rb2d.AddForce(transform.up * m_speed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_attachedPlayer != null)
        {
            if (m_rb2d.velocity.x < 0.01 && m_rb2d.velocity.x > -0.01 && m_rb2d.velocity.y < 0.01 && m_rb2d.velocity.y > -0.01)
            {
                m_attachedPlayer.TeleportToPosition(transform.position);
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {

        m_rb2d.drag = 3f;
        m_rb2d.angularDrag = 1f;
    }

    private void OnCollisionExit2D(Collision2D other)
    {

        m_rb2d.drag = 1f;
        m_rb2d.angularDrag = 0.05f;
    }
}
