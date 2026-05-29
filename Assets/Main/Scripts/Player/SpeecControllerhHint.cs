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
    }

    public void HideRecording()
    {
        _hintAnimator.SetTrigger("Stop");
        _recordingEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _loadingEffect.gameObject.SetActive(true);

        if (_cancelCoroutine != null)
            StopCoroutine(_cancelCoroutine);
    }

    public void WarnRecording(float timer)
    {
        if(timer <= _warningStartTime && !_isWarning)
        {
            _hintAnimator.SetTrigger("Warning");
            _cancelCoroutine = StartCoroutine(CancelRecording());
            _isWarning = true;
        }
    }

    public void ShowResponse()
    {
        _hintAnimator.SetTrigger("Response");
        _loadingEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _itemAppearanceEffect.gameObject.SetActive(true);
    }

    private IEnumerator CancelRecording()
    {
        yield return new WaitForSeconds(_warningStartTime);
        _recordingEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
