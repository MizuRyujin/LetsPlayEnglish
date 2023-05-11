using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private AudioSource[] _sources;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Plays an AudioClip on a specific AudioSource
    /// </summary>
    /// <param name="clip">The clip to be played.</param>
    /// <param name="type">Type of AudioSource (SFX or others). Default as Default Type.</param>
    /// <param name="cutPrevious">Should the previous clip be stopped. Default as False.</param>
    public void PlayClip(AudioClip clip, AudioSourceType type = AudioSourceType.Default, bool cutPrevious = false)
    {
        // Behave differently if a source type is specified
        switch (type)
        {
            case AudioSourceType.SFX:
                if (!_sources[1].isPlaying || cutPrevious)
                {
                    _sources[1].PlayOneShot(clip);
                }
                break;

            default:
                if (!_sources[0].isPlaying)
                {
                    _sources[0].PlayOneShot(clip);
                }
                break;
        }
    }
}

public enum AudioSourceType
{
    Default,
    Environment,
    SFX
}
