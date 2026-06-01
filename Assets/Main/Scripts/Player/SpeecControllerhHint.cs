using System.Collections;
using UnityEngine;

public class SpeecControllerhHint : MonoBehaviour
{
    [SerializeField] private HintButton _hintButton;
    [SerializeField] private ParticleSystem _recordingEffect;
    [SerializeField] private ParticleSystem _loadingEffect;
    [SerializeField] private GameObject _itemAppearanceEffect;
    [SerializeField] private Animator _hintAnimator;
    [SerializeField] private float _warningStartTime = 3;
    [SerializeField] private float _errorEffectGravity = 1;

    private bool _isWarning;
    private Coroutine _cancelCoroutine;

    public void ToggleHint(bool enable)
    {
        _hintAnimator.gameObject.SetActive(enable);
        _hintButton.ToggleHint(enable);
    }

    public void ShowRecording()
    {
        _isWarning = false;
        _hintAnimator.SetTrigger("Start");
        _recordingEffect.gameObject.SetActive(true);
        _recordingEffect.Play();
    }

    public void HideRecording()
    {
        _hintAnimator.SetTrigger("Send");
        _hintButton.ToggleDisable(true);
        _recordingEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        var loadingMain = _loadingEffect.main;
        loadingMain.gravityModifierMultiplier = 0;
        _loadingEffect.gameObject.SetActive(true);
        _loadingEffect.Play();

        if (_cancelCoroutine != null)
            StopCoroutine(_cancelCoroutine);
    }

    public void CancelRecording()
    {
        _hintAnimator.SetTrigger("Stop");
        _recordingEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public void WarnRecording(float timer)
    {
        if(timer <= _warningStartTime && !_isWarning)
        {
            _hintAnimator.SetTrigger("Warning");
            _cancelCoroutine = StartCoroutine(StopRecording());
            _isWarning = true;
        }
    }

    public void ShowResponse()
    {
        _hintAnimator.SetTrigger("Response");
        _hintButton.ToggleDisable(false);
        _loadingEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _itemAppearanceEffect.gameObject.SetActive(true);
    }

    public void ShowError()
    {
        _hintAnimator.SetTrigger("Error");
        _hintButton.ToggleDisable(false);

        var loadingMain = _loadingEffect.main;
        loadingMain.gravityModifierMultiplier = _errorEffectGravity;

        _loadingEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    private IEnumerator StopRecording()
    {
        yield return new WaitForSeconds(_warningStartTime);
        _recordingEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
