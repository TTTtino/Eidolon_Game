using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public PlayerController m_attachedPlayer;
    // Position projectiles are instantiated at
    [SerializeField] Transform m_shootTransform;
    // speed at which the weapon moves to the player
    [SerializeField] float m_moveToPlayerSpeed;
    // position to move to when following player
    private Vector3 m_targetPosition;
    // offset from player that the weapon stays at
    [SerializeField] Vector2 m_pOffset;
    // current projectile that is active
    public int m_projectileIndex = 0;
    // Projectles that are available
    private List<Projectile> m_projectiles = null;
    public List<Projectile> Projectiles { get { return m_projectiles; } set { m_projectiles = value; } }
    // active projectile
    public Projectile m_currentProjectile = null;
    // Layers that the projectile can be instantiated inside
    public LayerMask m_shootThroughLayers;
    // UI Image that shows the active projectile
    [SerializeField] Image m_currentProjectileUI;

    // Start is called before the first frame update
    void Start()
    {
        if (m_attachedPlayer != null && m_currentProjectile != null)
        {
            m_currentProjectileUI.sprite = m_currentProjectile.GetComponentInChildren<SpriteRenderer>().sprite;
            m_currentProjectileUI.color = Color.white;
        }
        else
        {
            m_currentProjectileUI.color = Color.black;
        }
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
            m_targetPosition = (Vector2)m_attachedPlayer.transform.position - offset;

            transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, m_moveToPlayerSpeed * (Vector3.Distance(m_targetPosition, transform.position) / 50f));
        }

    }

    public void SwitchProjectile(int index)
    {
        if (index < m_projectiles.Count && m_attachedPlayer != null)
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
        if (!Physics2D.OverlapCircle(m_shootTransform.position, 0.05f, m_shootThroughLayers))
        {
            Projectile p = Instantiate(m_currentProjectile, m_shootTransform.position, transform.rotation);
            // p.addSpawnVelocity(m_attachedPlayer.GetRBVelocity());
        }

    }

    // player picked up weapon
    public void PlayerPickedUp(PlayerController player)
    {
        m_attachedPlayer = player;
        Debug.Log(m_projectiles);
        if (m_projectiles.Count > 0) m_currentProjectile = m_projectiles[0];
        if (m_currentProjectile != null)
        {
            m_currentProjectileUI.sprite = m_currentProjectile.GetComponentInChildren<SpriteRenderer>().sprite;
            m_currentProjectileUI.color = Color.white;
        }
    }
    // player picked up weapon
    public void ProjectilePickedUp(Projectile projectile)
    {
        m_projectiles.Add(projectile);
        if (m_currentProjectile == null)
        {
            m_currentProjectile = projectile;
            m_currentProjectileUI.sprite = m_currentProjectile.GetComponentInChildren<SpriteRenderer>().sprite;
            m_currentProjectileUI.color = Color.white;
        }
    }
}
