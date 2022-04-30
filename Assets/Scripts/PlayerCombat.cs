using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour, IHittable
{
    // Start is called before the first frame update
    private PlayerStats m_stats;
    public PlayerStats Stats { get { return m_stats; } }
    [SerializeField] private Weapon m_activeWeapon;

    private float m_lastAttackTime = -1000f;
    [SerializeField] private float m_attackInterval = 100f;
    private bool m_mustAttack = false;

    void Awake()
    {
        m_stats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        m_attackInterval = m_activeWeapon.m_currentProjectile.GetCooldown();
        if (m_lastAttackTime + m_attackInterval < Time.time && m_mustAttack)
        {
            m_lastAttackTime = Time.time;
            m_activeWeapon.FireProjectile();
        }
        Vector2 aimDir = Mouse.current.position.ReadValue() - (Vector2)Camera.main.WorldToScreenPoint(m_activeWeapon.transform.position);
        aimDir.Normalize();
        if (m_activeWeapon != null)
        {
            float angle = Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg;
            m_activeWeapon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
        }
    }

    public void FireWeapon(InputAction.CallbackContext context)
    {
        if (m_activeWeapon != null)
        {
            if (context.started)
            {
                m_mustAttack = true;
            }
            else if (context.canceled)
            {
                m_mustAttack = false;
            }
        }
    }

    public void SwitchWeapon(InputAction.CallbackContext context)
    {
        if (m_activeWeapon != null)
        {
            if (context.started)
            {
                int x = 0;

                if (context.ReadValue<float>() > 0)
                {
                    x += 1;
                }
                else if (context.ReadValue<float>() < 0)
                {
                    x -= 1;
                }

                int m = (m_activeWeapon.m_projectileIndex + x) % m_activeWeapon.m_projectiles.Length;
                if (m < 0)
                {
                    m += m_activeWeapon.m_projectiles.Length;
                }
                m_activeWeapon.SwitchProjectile(m);
            }
        }
    }

    public void OnHit(int damageDealt)
    {
        m_stats.ReduceHealth(damageDealt);
        if (m_stats.Health <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
