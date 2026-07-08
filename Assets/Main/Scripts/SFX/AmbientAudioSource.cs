using System.Collections;
using UnityEngine;

/// <summary>
/// Plays ambient and changes it zone by zone.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AmbientAudioSource : ZoneAudioSource, IPausable
{
    private AudioSource _audioSource;
    [SerializeField] private float _fadeDuration = 3f;
    private float _defaultVolume;
    private Coroutine _switchCoroutine;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _defaultVolume = _audioSource.volume;
    }

    public override void ChangeClip(AudioClip nextClip)
    {
        if (!_audioSource.clip || !_audioSource.clip.Equals(nextClip))
        {
            StopSwitch();
            _switchCoroutine = StartCoroutine(SwitchAmbient(nextClip, _fadeDuration));
        }
    }

    /// <summary>
    /// Stops ambient with fade out effect.
    /// </summary>
    /// <param name="fadeDuration">Fade out duration in seconds.</param>
    public void StopAmbient(float fadeDuration)
    {
        StopSwitch();
        _switchCoroutine = StartCoroutine(StopAmbientRoutine(fadeDuration));
    }

    /// <summary>
    /// Stops ambient with fade out effect.
    /// </summary>
    public void StopAmbient()
    {
        StopAmbient(_fadeDuration);
    }

    /// <summary>
    /// Starts ambient with fade in effect.
    /// </summary>
    /// <param name="fadeDuration">Fade in duration in seconds.</param>
    public void PlayAmbient(float fadeDuration)
    {
        StopSwitch();
        _switchCoroutine = StartCoroutine(PlayAmbientRoutine(fadeDuration));
    }

    /// <summary>
    /// Starts ambient with fade in effect.
    /// </summary>
    public void PlayAmbient()
    {
        PlayAmbient(_fadeDuration);
    }

    /// <summary>
    /// Swithes ambient with smooth volume descrease and increase.
    /// </summary>
    /// <param name="ambient">Ambient to replace the currently playing one with.</param>
    /// <param name="fadeDuration">Fade in and out duration in seconds.</param>
    /// <returns></returns>
    private IEnumerator SwitchAmbient(AudioClip ambient, float fadeDuration)
    {
        if (_audioSource.clip)
            yield return StopAmbientRoutine(fadeDuration, false);
        else
            _audioSource.volume = 0;

        _audioSource.clip = ambient;
        yield return PlayAmbientRoutine(fadeDuration);
    }

    /// <summary>
    /// Smoothly decreases volume and stops ambient.
    /// </summary>
    /// <param name="fadeDuration">Fade out duration in seconds.</param>
    /// <param name="emptyCoroutine">If true, resets coroutine after its completion.</param>
    /// <returns></returns>
    private IEnumerator StopAmbientRoutine(float fadeDuration, bool emptyCoroutine = true)
    {
        yield return SmoothVolume(0, fadeDuration);
        _audioSource.Stop();

        if (emptyCoroutine)
            _switchCoroutine = null;
    }

    /// <summary>
    /// Plays ambient and smoothly increases volume.
    /// </summary>
    /// <param name="fadeDuration">Fade in duration in seconds.</param>
    /// <param name="emptyCoroutine">If true, resets coroutine after its completion.</param>
    /// <returns></returns>
    private IEnumerator PlayAmbientRoutine(float fadeDuration, bool emptyCoroutine = true)
    {
        _audioSource.Play();
        yield return SmoothVolume(_defaultVolume, fadeDuration);

        if (emptyCoroutine)
            _switchCoroutine = null;
    }

    /// <summary>
    /// Smoothly changes volume from current ot target value.
    /// </summary>
    /// <param name="targetVolume">Volume value to change to.</param>
    /// <param name="duration">Volume change duration in seconds.</param>
    /// <returns></returns>
    private IEnumerator SmoothVolume(float targetVolume, float duration)
    {
        var from = _audioSource.volume;
        var timer = 0f;

        if (from == targetVolume)
            yield break;

        while (timer < duration)
        {
            _audioSource.volume = Mathf.Lerp(from, targetVolume, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        _audioSource.volume = targetVolume;
    }

    /// <summary>
    /// Stops ambient switch coroutine.
    /// </summary>
    private void StopSwitch()
    {
        if(_switchCoroutine != null )
        {
            StopCoroutine(_switchCoroutine);
            _switchCoroutine = null;
        }
    }

    /// <summary>
    /// Pauses ambient;
    /// </summary>
    public void Freeze()
    {
        _audioSource.Pause();
    }

    /// <summary>
    /// Resumes ambient;
    /// </summary>
    public void Unfreeze()
    {
        _audioSource.UnPause();
    }
}
