using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    public AudioClip[] footstepClips;
    private AudioSource m_audioSource;
    PlayerController m_playerController;
    private float m_time;

    // Start is called before the first frame update
    void Start()
    {
        m_playerController = GetComponentInParent<PlayerController>();
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayFootStepAudio()
    {
        m_audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
    }
}
