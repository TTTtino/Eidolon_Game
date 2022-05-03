using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
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
    public TMP_Text m_timeElapsedText;
    private bool m_levelComplete;
    public float m_levelStartTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        Debug.Log("Awake called, level controller");

    }



    void Start()
    {
        Debug.Log("Start called, level controller");
        m_levelStartTime = Time.time;
        m_weapon.Projectiles = m_weaponProjectiles;
        m_playerStats.MaxHealth = m_playerMaxHealth;
        if (m_playerHasWeapon)
        {
            m_weapon.PlayerPickedUp(m_playerController);
            m_playerCombat.WeaponPickedUp(m_weapon);
        }
        StarCollected(0);
    }

    public void LevelComplete()
    {
        float timeTaken = Time.time - m_levelStartTime;
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

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void Update()
    {
        if (!m_levelComplete && m_playerController.m_controllable)
        {
            float timeTaken = Time.time - m_levelStartTime;
            if (timeTaken > 0)
            {
                int mins = (int)(timeTaken / 60.0f);
                int secs = (int)(timeTaken % 60);

                m_timeElapsedText.SetText("<b> " + mins + "</b>:<b>" + secs + "</b>");

            }
            else
            {
                m_timeElapsedText.SetText("--:--");
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

    public void StarCollected(int numCollected)
    {
        for (int i = 0; i < m_lvlStars.Length; i++)
        {
            if (i + 1 > numCollected)
            {
                m_lvlStars[i].color = new Color32(20, 20, 20, 255);
            }
            else
            {
                m_lvlStars[i].color = new Color32(255, 255, 255, 255);
            }
        }
    }



}
