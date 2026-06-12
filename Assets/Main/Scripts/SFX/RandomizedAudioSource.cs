using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomizedAudioSource : MonoBehaviour
{
    [SerializeField] private float _pitchDifferance = 0.2f;
    [SerializeField] private float _volumeDifferance = 0.1f;
    private float _defaultPitch, _defaultVolume;
    private AudioSource _audioSource;

    public bool IsPlaying => _audioSource.isPlaying;

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

    public void Play()
    {
        _audioSource.Stop();
        _audioSource.pitch = Random.Range(-_pitchDifferance, _pitchDifferance) + _defaultPitch;
        _audioSource.volume = Random.Range(-_volumeDifferance, _volumeDifferance) + _defaultVolume;
        _audioSource.Play();
    }

    public void Play(AudioClip clip)
    {
        _audioSource.Stop();
        Clip = clip;
        Play();
    }

    public void Stop()
    {
        _audioSource.Stop();
    }
}
