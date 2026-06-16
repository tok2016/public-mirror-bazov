using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioClip _defaultAmbient;
    [SerializeField] private AudioZone[] _musicZones;
    private AudioSource _audioSource;
    private Coroutine _musicCoroutine;

    [Header("Steps")]
    [SerializeField] private TeleportationProvider _teleportationProvider;
    [SerializeField] private RandomizedAudioSource _playerStepsSourse;
    [SerializeField] private AudioClip _defaultStepSound;
    [SerializeField] private AudioZone[] _stepZones;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        foreach (var zone in _musicZones)
        {
            zone.OnPlayerEnter += ChangeAmbient;
            zone.OnPlayerExit += ResetAmbient;
        }

        foreach (var zone in _stepZones)
        {
            zone.OnPlayerEnter += ChangeStepSound;
            zone.OnPlayerExit += ResetStepSound;
        }

        _teleportationProvider.locomotionEnded += PlayStepSound;
    }

    void Start()
    {
        ResetStepSound();
    }

    public void PlayStepSound(LocomotionProvider provider)
    {
        _playerStepsSourse.Play();
    }

    public void ChangeStepSound(AudioClip sound)
    {
        if(!_playerStepsSourse.Clip || !_playerStepsSourse.Clip.Equals(sound))
            _playerStepsSourse.Clip = sound;
    }

    public void ResetStepSound()
    {
        ChangeStepSound(_defaultStepSound);
    }

    public void ChangeAmbient(AudioClip ambient)
    {
        if (!_audioSource.clip || !_audioSource.clip.Equals(ambient))
        {
            _audioSource.Stop();
            _audioSource.clip = ambient;
            _audioSource.Play();
        }
    }

    public void ResetAmbient()
    {
        ChangeAmbient(_defaultAmbient);
    }

    private IEnumerator SwitchAmbient(AudioClip ambient)
    {
        yield return null;
        if (!_audioSource.clip || !_audioSource.clip.Equals(ambient))
        {
            _audioSource.Stop();
            _audioSource.clip = ambient;
            _audioSource.Play();
        }
    }

    private void OnDisable()
    {
        foreach (var zone in _musicZones)
        {
            zone.OnPlayerEnter -= ChangeAmbient;
            zone.OnPlayerExit -= ResetAmbient;
        }

        foreach (var zone in _stepZones)
        {
            zone.OnPlayerEnter -= ChangeStepSound;
            zone.OnPlayerExit -= ResetStepSound;
        }

        _teleportationProvider.locomotionEnded -= PlayStepSound;
    }
}
