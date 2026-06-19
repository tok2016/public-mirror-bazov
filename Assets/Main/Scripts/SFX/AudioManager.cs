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
    [SerializeField] private float _fadeDuration = 3f;
    private AudioSource _audioSource;
    private float _defaultVolume;
    private Coroutine _switchCoroutine;

    [Header("Steps")]
    [SerializeField] private TeleportationProvider _teleportationProvider;
    [SerializeField] private RandomizedAudioSource _playerStepsSourse;
    [SerializeField] private AudioClip _defaultStepSound;
    [SerializeField] private AudioZone[] _stepZones;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _defaultVolume = _audioSource.volume;
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

    public void StopAmbient(float fadeDuration)
    {
        if(_switchCoroutine != null)
            StopCoroutine(_switchCoroutine);
        _switchCoroutine = StartCoroutine(StopAmbientRoutine(fadeDuration));
    }

    public void PlayAmbient(float fadeDuration)
    {
        if (_switchCoroutine != null)
            StopCoroutine(_switchCoroutine);
        _switchCoroutine = StartCoroutine(PlayAmbientRoutine(fadeDuration));
    }

    public void ChangeAmbient(AudioClip ambient, float fadeDuration)
    {
        if (!_audioSource.clip || !_audioSource.clip.Equals(ambient))
        {
            if (_switchCoroutine != null)
                StopCoroutine(_switchCoroutine);
            _switchCoroutine = StartCoroutine(SwitchAmbient(ambient, fadeDuration));
        }
    }

    public void ChangeAmbient(AudioClip ambient)
    {
        ChangeAmbient(ambient, _fadeDuration);
    }

    public void ResetAmbient(float fadeDuration)
    {
        ChangeAmbient(_defaultAmbient, fadeDuration);
    }

    public void ResetAmbient()
    {
        ResetAmbient(_fadeDuration);
    }

    private IEnumerator SwitchAmbient(AudioClip ambient, float fadeDuration)
    {
        if (_audioSource.clip)
            yield return StopAmbientRoutine(fadeDuration, false);
        else
            _audioSource.volume = 0;

        _audioSource.clip = ambient;
        yield return PlayAmbientRoutine(fadeDuration);
    }

    private IEnumerator StopAmbientRoutine(float fadeDuration, bool emptyCoroutine = true)
    {
        yield return SmoothVolume(0, fadeDuration);
        _audioSource.Stop();

        if(emptyCoroutine)
            _switchCoroutine = null;
    }

    private IEnumerator PlayAmbientRoutine(float fadeDuration, bool emptyCoroutine = true)
    {
        _audioSource.Play();
        yield return SmoothVolume(_defaultVolume, fadeDuration);

        if(emptyCoroutine)
            _switchCoroutine = null;
    }

    private IEnumerator SmoothVolume(float targetVolume, float duration)
    {
        var from = _audioSource.volume;
        var timer = 0f;

        if(from == targetVolume)
            yield break;

        while(timer < duration)
        {
            _audioSource.volume = Mathf.Lerp(from, targetVolume, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        _audioSource.volume = targetVolume;
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
