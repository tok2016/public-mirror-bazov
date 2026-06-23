using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    private Coroutine _loader;

    public void FadeIn(float duration)
    {
        if (_loader == null)
            _loader = StartCoroutine(Fade(1, duration));
    }

    public void FadeOut(float duration)
    {
        if(_loader != null)
            StopCoroutine(_loader);
        else
            _loader = StartCoroutine(Fade(0, duration));
    }

    public void FadeOutFromStart(float duration)
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;

        FadeOut(duration);
    }

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
