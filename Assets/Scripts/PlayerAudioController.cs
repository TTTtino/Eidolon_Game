using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Audio controller for player
public class PlayerAudioController : MonoBehaviour
{
    // List of audio that would be chosen from for each footstep
    public AudioClip[] footstepClips;
    private AudioSource m_audioSource;
    PlayerController m_playerController;

    // Start is called before the first frame update
    void Start()
    {
        m_playerController = GetComponentInParent<PlayerController>();
        m_audioSource = GetComponent<AudioSource>();
    }


    public void PlayFootStepAudio()
    {
        m_audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
    }
}
