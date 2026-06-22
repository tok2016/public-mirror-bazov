using UnityEngine;

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

    public void Freeze()
    {
        _particles.Pause();
        _audioSource?.Pause();
    }

    public void Unfreeze()
    {
        _particles.Play();
        _audioSource?.UnPause();
    }
}
