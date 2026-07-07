using UnityEngine;

/// <summary>
/// Wraps up <c>AudioSource</c> to randomize its play.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class RandomizedAudioSource : MonoBehaviour
{
    [SerializeField] private float _pitchDifferance = 0.2f;
    [SerializeField] private float _volumeDifferance = 0.1f;
    private float _defaultPitch, _defaultVolume;
    private AudioSource _audioSource;

    public bool IsPlaying => _audioSource.isPlaying;

    /// <summary>
    /// Stops previous clip and replaces it with given one.
    /// </summary>
    /// <value>
    /// Currently playing audio clip.
    /// </value>
    public AudioClip Clip
    {
        get => _audioSource.clip;
        set
        {
            _audioSource.Stop();
            _audioSource.clip = value;
        }
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _defaultPitch = _audioSource.pitch;
        _defaultVolume = _audioSource.volume;
    }

    /// <summary>
    /// Plays current audio clip with random volume and pitch.
    /// </summary>
    public void Play()
    {
        _audioSource.Stop();
        _audioSource.pitch = Random.Range(-_pitchDifferance, _pitchDifferance) + _defaultPitch;
        _audioSource.volume = Random.Range(-_volumeDifferance, _volumeDifferance) + _defaultVolume;
        _audioSource.Play();
    }

    /// <summary>
    /// Changes audio clip to given one and plays it.
    /// </summary>
    /// <param name="clip"></param>
    public void Play(AudioClip clip)
    {
        _audioSource.Stop();
        Clip = clip;
        Play();
    }

    /// <summary>
    /// Stops audio source.
    /// </summary>
    public void Stop()
    {
        _audioSource.Stop();
    }
}
