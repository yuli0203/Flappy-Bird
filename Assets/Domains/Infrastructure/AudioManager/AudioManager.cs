
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void Play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
