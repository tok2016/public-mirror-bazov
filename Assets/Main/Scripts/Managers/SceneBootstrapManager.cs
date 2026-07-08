using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Manages scene loading. 
/// </summary>
public class SceneBootstrapManager : MonoBehaviour
{
    [SerializeField] private bool _fadeOutOnAwake;
    [SerializeField] private float _fadeDuration = 3f;
    [SerializeField] private FadeScreen _fadeScreen;
    [SerializeField] private AmbientAudioSource _ambientSource;
    [SerializeField] private QuestManager _questManager;
    [SerializeField] private Pause _pause;
    [SerializeField] private XRInteractionManager _interactionManager;
    private Coroutine _loader;

    /// <summary>
    /// Activates opaque fade screen and fade out effect on scene start.
    /// </summary>
    private void Start()
    {
        if (_fadeOutOnAwake)
        {
            _fadeScreen.FadeOutFromStart(_fadeDuration);
            _questManager.Delay(_fadeDuration);
        }

        _ambientSource.ResetClip();
    }

    /// <summary>
    /// Starts loading the scene of given index.
    /// Opens given scene after its loading completion.
    /// </summary>
    /// <param name="sceneIndex">Scene to load.</param>
    public void LoadScene(int sceneIndex)
    {
        if (_loader == null)
            _loader = StartCoroutine(StartLoadScene(sceneIndex));
    }

    /// <summary>
    /// Waits for given scene to load with smooth fade in / out effects.
    /// </summary>
    /// <remarks>
    /// Disables pause and all XR interactions.
    /// </remarks>
    /// <param name="sceneIndex">Scene to load.</param>
    /// <returns></returns>
    private IEnumerator StartLoadScene(int sceneIndex)
    {
        _fadeScreen.FadeIn(_fadeDuration);
        _interactionManager.enabled = false;
        _pause.enabled = false;
        _ambientSource.StopAmbient(_fadeDuration);
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
