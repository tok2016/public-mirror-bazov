using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneBootstrapManager : MonoBehaviour
{
    [SerializeField] private bool _fadeOutOnAwake;
    [SerializeField] private float _fadeDuration = 3f;
    [SerializeField] private FadeScreen _fadeScreen;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private QuestManager _questManager;
    [SerializeField] private Pause _pause;
    [SerializeField] private XRInteractionManager _interactionManager;
    private Coroutine _loader;

    private void Start()
    {
        if (_fadeOutOnAwake)
        {
            _fadeScreen.FadeOutFromStart(_fadeDuration);
            _questManager.Delay(_fadeDuration);
        }
            
        _audioManager.ResetAmbient(_fadeDuration);
    }

    public void LoadScene(int sceneIndex)
    {
        if (_loader == null)
            _loader = StartCoroutine(StartLoadScene(sceneIndex));
    }

    private IEnumerator StartLoadScene(int sceneIndex)
    {
        _fadeScreen.FadeIn(_fadeDuration);
        _interactionManager.enabled = false;
        _pause.enabled = false;
        _audioManager.StopAmbient(_fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
        var sceneLoader = SceneManager.LoadSceneAsync(sceneIndex);
        sceneLoader.allowSceneActivation = false;

        while(sceneLoader.progress < 0.9f)
            yield return null;

        _interactionManager.enabled = true;
        _pause.enabled = true;
        sceneLoader.allowSceneActivation = true;
        _loader = null;
    }
}
