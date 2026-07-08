using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

/// <summary>
/// Plays step sound on teleportation and changes it zone by zone.
/// </summary>
[RequireComponent(typeof(RandomizedAudioSource))]
public class StepAudioSource : ZoneAudioSource
{
    [SerializeField] private LocomotionProvider _teleportationProvider;
    private RandomizedAudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<RandomizedAudioSource>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _teleportationProvider.locomotionEnded += OnTeleport;
    }

    /// <summary>
    /// Plays step sound zone by zone.
    /// </summary>
    /// <param name="provider"></param>
    private void OnTeleport(LocomotionProvider provider)
    {
        _audioSource.Stop();
        _audioSource.Play();
    }

    public override void ChangeClip(AudioClip nextClip)
    {
        if (!_audioSource.Clip || !_audioSource.Clip.Equals(nextClip))
            _audioSource.Clip = nextClip;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _teleportationProvider.locomotionEnded -= OnTeleport;
    }
}
