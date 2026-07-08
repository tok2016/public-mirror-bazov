using UnityEngine;

/// <summary>
/// Wraps up <c>AudioSource</c> to freeze it on pause.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PausableAudioSource : MonoBehaviour, IPausable
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Pause.onPause += Freeze;
        Pause.onContinue += Unfreeze;
    }

    /// <summary>
    /// Pauses audio source.
    /// </summary>
    public void Freeze()
    {
        _audioSource.Pause();
    }

    /// <summary>
    /// Resumes audio source.
    /// </summary>
    public void Unfreeze()
    {
        _audioSource.UnPause();
    }

    private void OnDisable()
    {
        Pause.onPause -= Freeze;
        Pause.onContinue -= Unfreeze;
    }
}
