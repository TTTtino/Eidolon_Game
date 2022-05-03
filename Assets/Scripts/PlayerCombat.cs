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
    public Weapon m_activeWeapon;
    public PlayerController m_controller;

    private float m_lastAttackTime = -1000f;
    [SerializeField] private float m_attackInterval = 100f;
    private bool m_mustAttack = false;

    void Awake()
    {
        m_stats = GetComponent<PlayerStats>();
        m_controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_activeWeapon != null && m_controller.m_controllable)
        {
            if (m_activeWeapon.m_currentProjectile != null)
            {
                m_attackInterval = m_activeWeapon.m_currentProjectile.GetCooldown();
                if (m_lastAttackTime + m_attackInterval < Time.time && m_mustAttack)
                {
                    m_lastAttackTime = Time.time;
                    m_activeWeapon.FireProjectile();
                }

            }
            Vector2 aimDir = Mouse.current.position.ReadValue() - (Vector2)Camera.main.WorldToScreenPoint(m_activeWeapon.transform.position);
            aimDir.Normalize();
            float angle = Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg;
            m_activeWeapon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
        }
    }

    public void FireWeapon(InputAction.CallbackContext context)
    {
        if (m_activeWeapon != null && m_controller.m_controllable)
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
        if (m_activeWeapon != null && m_activeWeapon.Projectiles.Count > 0 && m_controller.m_controllable)
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

                int m = (m_activeWeapon.m_projectileIndex + x) % m_activeWeapon.Projectiles.Count;
                if (m < 0)
                {
                    m += m_activeWeapon.Projectiles.Count;
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

    public void WeaponPickedUp(Weapon weapon)
    {
        m_activeWeapon = weapon;
    }
}
