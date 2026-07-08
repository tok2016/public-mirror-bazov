using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Manages main starting scene.
/// </summary>
public class MainScene : MonoBehaviour
{
    [SerializeField] private TeleportPad _padToActivate;
    [SerializeField] private InputActionReference _startAction;
    [SerializeField] private ActionsVisualizer[] _visualizers;
    [SerializeField] private Button _quitButton;

    private void OnEnable()
    {
        _startAction.action.performed += StartQuest;
        _quitButton.onClick.AddListener(Quit);
    }

    void Start()
    {
        StartCoroutine(Wait());
    }

    /// <summary>
    /// Shows information about buttons as in a pause in the next frame.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Wait()
    {
        yield return null;
        foreach (var visualizer in _visualizers)
            visualizer.WarnAboutPause(true);
    }

    /// <summary>
    /// Activates portal to the first scene of quest.
    /// </summary>
    /// <param name="context"></param>
    public void StartQuest(InputAction.CallbackContext context)
    {
        _padToActivate.Activate();
        _padToActivate.EnterThePad();
    }

    /// <summary>
    /// Closes application.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    private void OnDisable()
    {
        _startAction.action.performed -= StartQuest;
        _quitButton.onClick.RemoveListener(Quit);
    }
}
