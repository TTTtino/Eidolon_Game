using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject m_startMenu;
    public GameObject m_LevelSelector;
    public void SwitchToLevelSelector()
    {
        m_startMenu.SetActive(false);
        m_LevelSelector.SetActive(true);
    }

    public void SwitchToStartMenu()
    {
        m_startMenu.SetActive(true);
        m_LevelSelector.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
