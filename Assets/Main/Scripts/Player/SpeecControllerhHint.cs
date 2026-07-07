using System.Collections;
using UnityEngine;

/// <summary>
/// Controlls hint system and visualization of recording.
/// </summary>
public class SpeecControllerhHint : MonoBehaviour, IPausable
{
    [SerializeField] private ActionsVisualizer _controller, _hand;
    [SerializeField] private ParticleSystem _recordingEffect;
    [SerializeField] private ParticleSystem _loadingEffect;
    [SerializeField] private GameObject _itemAppearanceEffect;
    [SerializeField] private Animator _hintAnimator;
    [SerializeField] private float _warningStartTime = 3;
    [SerializeField] private float _errorEffectGravity = 1;

    private bool _isWarning;
    private bool _wasActive;
    private Coroutine _cancelCoroutine;

    /// <summary>
    /// Enables or disables hint system.
    /// </summary>
    /// <remarks>
    /// Makes controller model translucent.
    /// </remarks>
    /// <param name="enable">Whether to enable or disable hint system.</param>
    public void ToggleHint(bool enable)
    {
        _hintAnimator.gameObject.SetActive(enable);

        if (enable)
            _controller.State = VisualizerState.Translucent;
        else if(!Pause.IsPaused)
            _controller.State = VisualizerState.Invisible;

        _controller.WarnAboutRecording(enable);
        _hand.WarnAboutRecording(enable);
    }

    /// <summary>
    /// Starts recording effects and animation.
    /// </summary>
    public void ShowRecording()
    {
        _isWarning = false;
        _hintAnimator.SetTrigger("Start");
        _recordingEffect.gameObject.SetActive(true);
        _recordingEffect.Play();
    }

    /// <summary>
    /// Starts loading effects and animation. Stops recording and pulsatile warning animation.
    /// </summary>
    public void HideRecording()
    {
        _hintAnimator.SetTrigger("Send");
        _recordingEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        _controller.DisableRecording(true);

        var loadingMain = _loadingEffect.main;
        loadingMain.gravityModifierMultiplier = 0;
        _loadingEffect.gameObject.SetActive(true);
        _loadingEffect.Play();

        if (_cancelCoroutine != null)
        {
            StopCoroutine(_cancelCoroutine);
            _cancelCoroutine = null;
        }
    }

    /// <summary>
    /// Stops recordging effects and animation.
    /// </summary>
    public void CancelRecording()
    {
        _hintAnimator.SetTrigger("Stop");
        _recordingEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    /// <summary>
    /// Starts pulsatile animation that warn about running out of recording time. 
    /// </summary>
    /// <param name="timer">Current recording time in seconds.</param>
    public void WarnRecording(float timer)
    {
        if(timer <= _warningStartTime && !_isWarning)
        {
            _hintAnimator.SetTrigger("Warning");
            _cancelCoroutine = StartCoroutine(StopRecording());
            _isWarning = true;
        }
    }

    /// <summary>
    /// Starts successs animation and throws loading particles.
    /// </summary>
    public void ShowResponse()
    {
        _hintAnimator.SetTrigger("Response");
        _controller.DisableRecording(false);
        _loadingEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _itemAppearanceEffect.gameObject.SetActive(true);
    }

    /// <summary>
    /// Starts error animation and makes loading particles fall down.
    /// </summary>
    public void ShowError()
    {
        _hintAnimator.SetTrigger("Error");
        _controller.DisableRecording(false);

        var loadingMain = _loadingEffect.main;
        loadingMain.gravityModifierMultiplier = _errorEffectGravity;

        _loadingEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    /// <summary>
    /// Stops recording effects and animation after delay.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StopRecording()
    {
        yield return new WaitForSeconds(_warningStartTime);
        _recordingEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        _cancelCoroutine = null;
    }

    /// <summary>
    /// Freezes hint animation.
    /// </summary>
    public void Freeze()
    {
        _hintAnimator.speed = 0;
        _wasActive = _hintAnimator.gameObject.activeInHierarchy;
        ToggleHint(false);
    }

    /// <summary>
    /// Continues hint animation.
    /// </summary>
    public void Unfreeze()
    {
        _hintAnimator.speed = 1;
        ToggleHint(_wasActive);
    }
}
