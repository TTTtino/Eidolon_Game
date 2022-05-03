using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public string m_levelName;
    public string m_levelLabel;
    public Image[] m_starImages;
    public bool m_levelUnlocked;
    // Start is called before the first frame update

    void Start()
    {
        int starsCollected = Mathf.Clamp(PlayerPrefs.GetInt(m_levelName + "Stars", 0), 0, 3);
        float secondsToComplete = PlayerPrefs.GetFloat(m_levelName + "Time", 0.0f);
        int levelUnlocked = PlayerPrefs.GetInt(m_levelName + "Unlocked", (m_levelUnlocked ? 1 : 0));

        if (levelUnlocked == 0)
        {
            GetComponent<Button>().interactable = false;
        }

        TMP_Text levelNameText = transform.Find("LevelName").GetComponent<TMP_Text>();
        levelNameText.SetText(m_levelLabel);

        for (int i = 0; i < m_starImages.Length; i++)
        {
            if (i + 1 > starsCollected)
            {
                m_starImages[i].color = new Color32(20, 20, 20, 255);
            }
            else
            {
                m_starImages[i].color = new Color32(255, 255, 255, 255);
            }
        }

        TMP_Text bestTimeText = transform.Find("BestTime").GetComponent<TMP_Text>();
        if (secondsToComplete > 0)
        {
            Debug.Log(m_levelLabel + " " + secondsToComplete);
            int mins = (int)(secondsToComplete / 60.0f);
            int secs = (int)(secondsToComplete % 60);

            bestTimeText.SetText("Best Time: <b> " + mins + "</b>:<b>" + secs + "</b>");

        }
        else
        {
            bestTimeText.SetText("Best Time: --:--");
        }

    }

    public void StartLevel()
    {
        SceneManager.LoadScene(m_levelName);
    }
}
