using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public string m_levelName;
    public Weapon m_weapon;
    public PlayerStats m_playerStats;
    public string m_nextLevelName;
    public PlayerController m_playerController;
    public PlayerCombat m_playerCombat;
    public GameObject m_levelCompleteCanvas;

    // Projectiles available in this level
    public List<Projectile> m_weaponProjectiles;
    public int m_playerMaxHealth = 10;
    public bool m_playerHasWeapon = true;
    public Image[] m_lvlCompleteStars;
    public Image[] m_lvlStars;

    void Awake()
    {
        m_weapon.Projectiles = m_weaponProjectiles;
        m_playerStats.MaxHealth = m_playerMaxHealth;
        if (m_playerHasWeapon)
        {
            m_weapon.PlayerPickedUp(m_playerController);
            m_playerCombat.WeaponPickedUp(m_weapon);
        }
    }

    public void LevelComplete()
    {
        float timeTaken = Time.timeSinceLevelLoad;
        m_levelCompleteCanvas.SetActive(true);
        m_playerController.m_controllable = false;

        for (int i = 0; i < m_lvlCompleteStars.Length; i++)
        {
            if (i + 1 > m_playerController.StarsCollected)
            {
                m_lvlCompleteStars[i].color = new Color32(20, 20, 20, 255);
            }
            else
            {
                m_lvlCompleteStars[i].color = new Color32(255, 255, 255, 255);
            }
        }

        int previousBestStars = Mathf.Clamp(PlayerPrefs.GetInt(m_levelName + "Stars", 0), 0, 3);
        float previousBestTime = PlayerPrefs.GetFloat(m_levelName + "Time", float.MaxValue);
        if (m_playerController.StarsCollected > previousBestStars)
        {
            PlayerPrefs.SetInt(m_levelName + "Stars", Mathf.Clamp(m_playerController.StarsCollected, 0, 3));
        }
        if (timeTaken < previousBestTime)
        {
            PlayerPrefs.SetFloat(m_levelName + "Time", timeTaken);
        }
        PlayerPrefs.SetInt(m_nextLevelName + "Unlocked", 1);
    }

    private void Update()
    {
        for (int i = 0; i < m_lvlStars.Length; i++)
        {
            if (i + 1 > m_playerController.StarsCollected)
            {
                m_lvlStars[i].color = new Color32(20, 20, 20, 255);
            }
            else
            {
                m_lvlStars[i].color = new Color32(255, 255, 255, 255);
            }
        }
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(m_nextLevelName);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

}
