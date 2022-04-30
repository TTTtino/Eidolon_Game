using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Weapon : MonoBehaviour
{
    [SerializeField] PlayerController m_attachedPlayer;
    [SerializeField] Transform m_shootTransform;
    [SerializeField] float m_moveToPlayerSpeed;
    Rigidbody2D m_rb2d;
    private Vector3 m_targetPosition;
    [SerializeField] Vector2 m_pOffset;
    public int m_projectileIndex = 0;
    public Projectile[] m_projectiles;
    public Projectile m_currentProjectile;


    // UI
    [SerializeField] Image m_currentProjectileUI;
    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        if (m_attachedPlayer != null)
        {
            m_currentProjectileUI.sprite = m_currentProjectile.GetComponentInChildren<SpriteRenderer>().sprite;
        }
        m_rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_attachedPlayer != null)
        {
            Vector2 offset = m_pOffset;
            if (m_attachedPlayer.m_isLeft)
            {
                offset.x *= -1;
            }
            m_targetPosition = m_attachedPlayer.m_rb2d.position - offset;

            m_rb2d.MovePosition(Vector3.MoveTowards(m_rb2d.position, m_targetPosition, m_moveToPlayerSpeed * (Vector3.Distance(m_targetPosition, m_rb2d.position) / 50f)));
        }

    }

    public void SwitchProjectile(int index)
    {
        if (index < m_projectiles.Length)
        {
            if (m_projectiles[index].m_isActive)
            {
                m_currentProjectile = m_projectiles[index];
                m_projectileIndex = index;
                m_currentProjectileUI.sprite = m_currentProjectile.GetComponentInChildren<SpriteRenderer>().sprite;
            }
        }
    }

    public void FireProjectile()
    {
        Projectile p = Instantiate(m_currentProjectile, m_shootTransform.position, transform.rotation);
        p.addSpawnVelocity(m_attachedPlayer.GetRBVelocity());

    }
}
