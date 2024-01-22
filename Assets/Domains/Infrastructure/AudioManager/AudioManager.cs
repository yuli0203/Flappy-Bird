using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private const int maxSimulteneousSounds = 4;

    [SerializeField] private AudioSource[] audioSources;

    private void Start()
    {
        // Initialize AudioSource components
        audioSources = new AudioSource[maxSimulteneousSounds];
        for (int i = 0; i < maxSimulteneousSounds; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Play(AudioClip clip)
    {
        // Find an available AudioSource to play the new clip
        AudioSource availableAudioSource = GetAvailableAudioSource();

        if (availableAudioSource != null)
        {
            availableAudioSource.clip = clip;
            availableAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("No available AudioSource to play the audio clip.");
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        // Find the first AudioSource that is not playing or is not active
        for (int i = 0; i < maxSimulteneousSounds; i++)
        {
            if (!audioSources[i].isPlaying || !audioSources[i].gameObject.activeInHierarchy)
            {
                return audioSources[i];
            }
        }

        return null;
    }
}
