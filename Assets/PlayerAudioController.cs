using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    public AudioClip footstepsClip;
    private AudioSource m_audioSource;
    PlayerController m_playerController;

    // Start is called before the first frame update
    void Start()
    {
        m_playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerController.ControlDirection != Vector2.zero)
        {
            m_audioSource.clip = footstepsClip;
        }
    }
}
