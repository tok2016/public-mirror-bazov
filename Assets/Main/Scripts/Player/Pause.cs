using System;
using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

/// <summary>
/// Manages game pause.
/// </summary>
public class Pause : MonoBehaviour
{
    [SerializeField] private InputActionReference _pauseAction;
    [SerializeField] private LocomotionProvider _locomotionProvider;
    [SerializeField] private XRBaseInteractor[] _teleportInteractors;
    [SerializeField] private XROrigin _origin;
    [SerializeField] private GameObject _pausePlace;
    [SerializeField] private Transform _pauseAttachPoint;

    /// <value>
    /// Is game paused.
    /// </value>
    public static bool IsPaused { get; private set; } = false;
    public static event Action onPause, onContinue;
    private Vector3 _prevPosition;

    [Header("Quit")]
    [SerializeField] private SceneBootstrapManager _sceneBootstrapManager;
    [SerializeField] private int _startScene;

    [Header("UI")]
    [SerializeField] private GameObject _confirmMenu;
    [SerializeField] private Button _quitButton, _confirmButton, _cancelButton;

    private void OnEnable()
    {
        _pauseAction.action.performed += ToggleActive;
        _quitButton.onClick.AddListener(OpenConfirmMenu);
        _confirmButton.onClick.AddListener(Quit);
        _cancelButton.onClick.AddListener(CloseConfirmMenu);
    }

    /// <summary>
    /// Pauses or continues the game. When paused, teleports player to the pause room in the scene.
    /// </summary>
    public void ToggleActive()
    {
        StartCoroutine(WaitBeforePause());
    }

    /// <summary>
    /// Pauses or continues the game. When paused, teleports player to the pause room in the scene.
    /// </summary>
    /// <param name="context"></param>
    public void ToggleActive(InputAction.CallbackContext context)
    {
        ToggleActive();
    }

    /// <summary>
    /// Executes pause in the next frame and teleports player to pause room.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitBeforePause()
    {
        yield return null;

        IsPaused = !IsPaused;
        _pausePlace.gameObject.SetActive(IsPaused);
        _locomotionProvider.enabled = !IsPaused;

        foreach (var interactor in _teleportInteractors)
            interactor.enabled = !IsPaused;

        if (IsPaused)
        {
            _prevPosition = _origin.transform.position;
            _origin.transform.position = _pauseAttachPoint.position;
            _origin.transform.rotation = _pauseAttachPoint.rotation;
            onPause?.Invoke();
        }
        else
        {
            _origin.transform.position = _prevPosition;
            onContinue?.Invoke();
        }
    }

    /// <summary>
    /// Loads main start scene.
    /// </summary>
    public void Quit()
    {
        _sceneBootstrapManager.LoadScene(_startScene);
    }

    /// <summary>
    /// Opens menu to confirm the quit.
    /// </summary>
    public void OpenConfirmMenu()
    {
        _confirmMenu.SetActive(true);
    }

    /// <summary>
    /// Closes confirm menu.
    /// </summary>
    public void CloseConfirmMenu()
    {
        _confirmMenu.SetActive(false);
    }

    private void OnDisable()
    {
        _pauseAction.action.performed -= ToggleActive;
        _pauseAction.action.performed -= ToggleActive;
        _quitButton.onClick.RemoveListener(OpenConfirmMenu);
        _confirmButton.onClick.RemoveListener(Quit);
        _cancelButton.onClick.RemoveListener(CloseConfirmMenu);
    }
}
