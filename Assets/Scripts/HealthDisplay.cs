using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 
public class HealthDisplay : MonoBehaviour
{
    public GameObject[] m_hearts;
    private PlayerStats m_playerStats;
    public Sprite filledHeart;
    public Sprite emptyHeart;


    private void Start()
    {
        m_playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (m_hearts.Length < m_playerStats.MaxHealth)
        {
            Debug.LogError("Not enough hearts in m_hearts for player max health.", this);
        }

        for (int i = 0; i < m_hearts.Length; i++)
        {
            if (i > m_playerStats.MaxHealth - 1)
            {
                m_hearts[i].SetActive(false);
            }
            else
            {
                m_hearts[i].SetActive(true);
            }

            if (i > m_playerStats.Health - 1)
            {
                m_hearts[i].GetComponent<Image>().sprite = emptyHeart;
            }
            else
            {
                m_hearts[i].GetComponent<Image>().sprite = filledHeart;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < m_playerStats.MaxHealth; i++)
        {
            if (i > m_playerStats.Health - 1)
            {
                m_hearts[i].GetComponent<Image>().sprite = emptyHeart;
            }
            else
            {
                m_hearts[i].GetComponent<Image>().sprite = filledHeart;
            }
        }
    }


}
