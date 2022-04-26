using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.InputSystem;
public class Weapon : MonoBehaviour
{
    [SerializeField] PlayerController m_attachedPlayer;
    [SerializeField] float m_moveToPlayerSpeed;
    private Vector3 m_targetPosition;
    Vector3 m_pOffset;
    public Projectile m_currentProjectile;
    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        m_pOffset = m_attachedPlayer.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = m_pOffset;
        if (m_attachedPlayer.m_isLeft)
        {
            offset.x *= -1;
        }
        m_targetPosition = m_attachedPlayer.transform.position - offset;

        transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, m_moveToPlayerSpeed * (Vector3.Distance(m_targetPosition, transform.position) / 5f) * Time.deltaTime);
    }




    public void FireProjectile()
    {
        Projectile p = Instantiate(m_currentProjectile, transform.position, transform.rotation);
        p.rb2d.velocity += m_attachedPlayer.m_rb2d.velocity;
    }
}
