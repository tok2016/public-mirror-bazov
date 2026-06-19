using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [field: SerializeField] public float FadeDuration { get; private set; }
    private Coroutine _loader;

    public void FadeIn()
    {
        if (_loader == null)
            _loader = StartCoroutine(Fade(1, FadeDuration));
    }

    public void FadeOut()
    {
        if(_loader != null)
            StopCoroutine(_loader);
        else
            _loader = StartCoroutine(Fade(0, FadeDuration));
    }

    public void FadeOutFromStart()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;

        FadeOut();
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
