using System.Collections;
using UnityEngine;

/// <summary>
/// Smoothly fades screen in front of camera in and out overlaying everything in the scene.
/// </summary>
public class FadeScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    private Coroutine _loader;

    /// <summary>
    /// Fades screen in, smoothly making it opaque.
    /// </summary>
    /// <param name="duration">Fade in duration in seconds.</param>
    public void FadeIn(float duration)
    {
        if (_loader == null)
            _loader = StartCoroutine(Fade(1, duration));
    }

    /// <summary>
    /// Fades screen out, smoothly making it transparent.
    /// </summary>
    /// <param name="duration">Fade out duration in seconds.</param>
    public void FadeOut(float duration)
    {
        if(_loader != null)
            StopCoroutine(_loader);
        else
            _loader = StartCoroutine(Fade(0, duration));
    }

    /// <summary>
    /// Fades screen out, starting with complete opaqueness.
    /// </summary>
    /// <param name="duration">Fade out duration in seconds.</param>
    public void FadeOutFromStart(float duration)
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;

        FadeOut(duration);
    }

    /// <summary>
    /// Smoothly changes screen alpha from current to target value.
    /// </summary>
    /// <param name="target">Target alpha value.</param>
    /// <param name="duration">Alpha change duration.</param>
    /// <returns></returns>
    private IEnumerator Fade(float target, float duration)
    {
        float start = _canvasGroup.alpha;
        float timer = 0;
        _canvasGroup.blocksRaycasts = target > 0.9f;

        while(timer < duration)
        {
            _canvasGroup.alpha = Mathf.Lerp(start, target, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        _canvasGroup.blocksRaycasts = target > 0.1f;
        _canvasGroup.alpha = target;
        _loader = null;
    }
}
