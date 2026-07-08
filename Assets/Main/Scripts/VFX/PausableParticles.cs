using UnityEngine;

/// <summary>
/// Wraps up <c>ParticleSystem</c> to freeze it on pause.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class PausableParticles : MonoBehaviour, IPausable
{
    private ParticleSystem _particles;
    private AudioSource _audioSource;

    private void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Pauses particles and their audio source.
    /// </summary>
    public void Freeze()
    {
        _particles.Pause();
        _audioSource?.Pause();
    }

    /// <summary>
    /// Resumes particle and their audio source.
    /// </summary>
    public void Unfreeze()
    {
        _particles.Play();
        _audioSource?.UnPause();
    }
}
