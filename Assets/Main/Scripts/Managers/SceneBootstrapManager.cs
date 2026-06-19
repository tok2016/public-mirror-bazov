using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBootstrapManager : MonoBehaviour
{
    [SerializeField] private FadeScreen _fadeScreen;
    [SerializeField] private AudioManager _audioManager;
    private Coroutine _loader;

    public void LoadScene(int sceneIndex)
    {
        if (_loader == null)
            _loader = StartCoroutine(StartLoadScene(sceneIndex));
    }

    private IEnumerator StartLoadScene(int sceneIndex)
    {
        _fadeScreen.FadeIn();
        _audioManager.ChangeVolume(0, _fadeScreen.FadeDuration);
        yield return new WaitForSeconds(_fadeScreen.FadeDuration);
        var sceneLoader = SceneManager.LoadSceneAsync(sceneIndex);
        sceneLoader.allowSceneActivation = false;

        while(sceneLoader.progress < 0.9f)
            yield return null;

        sceneLoader.allowSceneActivation = true;
        _loader = null;
    }
}
