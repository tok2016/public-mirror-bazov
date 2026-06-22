using UnityEngine;

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

    public void Freeze()
    {
        _audioSource.Pause();
    }

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
