using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;

/// <summary>
/// States of transparency.
/// </summary>
public enum VisualizerState
{
    Invisible = 0,
    Translucent = 1,
    Visible = 2
}

/// <summary>
/// Base class for all components that visualize input actions.
/// </summary>
public abstract class ActionsVisualizer : MonoBehaviour
{
    [SerializeField] protected XRBaseInteractor _grabInteractor, _pokeInteractor, _teleportInteractor;
    [SerializeField] protected NearFarInteractor _uiInteractor;
    [SerializeField] protected XRPokeInteractor _uiPokeInteractor;
    [SerializeField] protected SpeechController _speechController;
    [SerializeField] protected GameObject _objectWrapper;
    [SerializeField] protected VisualizerState _defaultState = VisualizerState.Visible;

    /// <summary>
    /// Changes dither value of visualizer's materials.
    /// </summary>
    /// <value>
    /// State of transparency of current visualizer.
    /// </value>
    public VisualizerState State
    {
        get => _defaultState;
        set
        {
            _defaultState = value;
            switch (value)
            {
                case VisualizerState.Invisible:
                    SetInvisible();
                    break;
                case VisualizerState.Translucent:
                    SetTranslucent();
                    break;
                default:
                    SetVisible();
                    break;
            }
        }
    }

    [SerializeField] protected Renderer[] _renderers;

    private void Start()
    {
        State = _defaultState;
    }

    protected virtual void OnEnable()
    {
        _grabInteractor?.hoverEntered.AddListener(OnGrabZoneEnter);
        _grabInteractor?.hoverExited.AddListener(OnGrabZoneExit);

        _grabInteractor?.selectEntered.AddListener(OnGrabStart);
        _grabInteractor?.selectExited.AddListener(OnGrabStop);

        if(_uiInteractor)
        {
            _uiInteractor.uiHoverEntered.AddListener(OnPokeZoneEnter);
            _uiInteractor.uiHoverExited.AddListener(OnPokeZoneExit);

            _uiInteractor.uiPressInput.inputActionReferencePerformed.action.performed += OnPokeStart;
            _uiInteractor.uiPressInput.inputActionReferencePerformed.action.canceled += OnPokeStop;
        }

        _uiPokeInteractor?.uiHoverEntered.AddListener(OnPokeZoneEnter);
        _uiPokeInteractor?.uiHoverExited.AddListener(OnPokeZoneExit);

        _pokeInteractor?.hoverEntered.AddListener(OnPokeZoneEnter);
        _pokeInteractor?.hoverExited.AddListener(OnPokeZoneExit);

        _teleportInteractor?.selectEntered.AddListener(OnTeleportStart);
        _teleportInteractor?.selectExited.AddListener(OnTeleportStop);

        if (_speechController)
        {
            _speechController.onRecordingStart += OnRecordingStart;
            _speechController.onRecordingStop += OnRecordingStop;
        }

        Pause.onPause += OnPauseStart;
        Pause.onContinue += OnPauseStop;
    }


    /// <summary>
    /// Shows that player can poke.
    /// </summary>
    /// <param name="enable">Whether to warn about poke or not.</param>
    public abstract void WarnAboutPoke(bool enable);

    /// <summary>
    /// Visualizes poke action.
    /// </summary>
    /// <param name="enable">Whether to show poke action or not.</param>
    public abstract void ShowPoke(bool enable);

    /// <summary>
    /// Shows that player can't poke.
    /// </summary>
    /// <param name="enable">Whether to show poke unavailability or not.</param>
    public abstract void DisablePoke(bool enable);


    /// <summary>
    /// Shows that player can grab.
    /// </summary>
    /// <param name="enable">Whether to warn about grab or not.</param>
    public abstract void WarnAboutGrab(bool enable);

    /// <summary>
    /// Visualizes grab action.
    /// </summary>
    /// <param name="enable">Whether to show grab action or not.</param>
    public abstract void ShowGrab(bool enable);

    /// <summary>
    /// Shows that player can't grab.
    /// </summary>
    /// <param name="enable">Whether to show grab unavailability or not.</param>
    public abstract void DisableGrab(bool enable);


    /// <summary>
    /// Shows that player can start recording.
    /// </summary>
    /// <param name="enable">Whether to warn about recording availability or not.</param>
    public abstract void WarnAboutRecording(bool enable);

    /// <summary>
    /// Visualizes recording action.
    /// </summary>
    /// <param name="enable">Whether to show record action or not.</param>
    public abstract void ShowRecording(bool enable);

    /// <summary>
    /// Shows that player can't start recording.
    /// </summary>
    /// <param name="enable">Whether to show record unavailability or not.</param>
    public abstract void DisableRecording(bool enable);


    /// <summary>
    /// Shows that player can teleport.
    /// </summary>
    /// <param name="enable">Whether to warn about teleport or not.</param>
    public abstract void WarnAboutTeleport(bool enable);

    /// <summary>
    /// Visualizes teleport action.
    /// </summary>
    /// <param name="enable">Whether to show teleport action or not.</param>
    public abstract void ShowTeleport(bool enable);

    /// <summary>
    /// Shows that player can't teleport.
    /// </summary>
    /// <param name="enable">Whether to show teleport unavailability or not.</param>
    public abstract void DisableTeleport(bool enable);


    /// <summary>
    /// Shows that player can play/pause.
    /// </summary>
    /// <param name="enable">Whether to warn about play/pause or not.</param>
    public abstract void WarnAboutPause(bool enable);

    /// <summary>
    /// Visualizes play/pause action.
    /// </summary>
    /// <param name="enable">Whether to show play/pause action or not.</param>
    public abstract void ShowPause(bool enable);

    /// <summary>
    /// Shows that player can't play/pause.
    /// </summary>
    /// <param name="enable">Whether to show play/pause unavailability or not.</param>
    public abstract void DisablePause(bool enable);

    /// <summary>
    /// Makes visualizer's materials transparent.
    /// </summary>
    private void SetInvisible()
    {
        foreach (var renderer in _renderers)
            renderer.material?.SetFloat("_Dither", 0);
            
    }

    /// <summary>
    /// Makes visualizer's materials translucent by setting dither value.
    /// </summary>
    private void SetTranslucent()
    {
        foreach (var renderer in _renderers)
            renderer.material?.SetFloat("_Dither", 1);
    }

    /// <summary>
    /// Makes visualizer's materials opaque.
    /// </summary>
    private void SetVisible()
    {
        foreach (var renderer in _renderers)
            renderer.material?.SetFloat("_Dither", 2);
    }


    /// <summary>
    /// Handles entrance to poke zone. Warns about poke.
    /// </summary>
    /// <param name="args"></param>
    private void OnPokeZoneEnter(UIHoverEventArgs args) => WarnAboutPoke(true);

    /// <summary>
    /// Handles entrance to poke zone. Warns about poke.
    /// </summary>
    /// <param name="args"></param>
    private void OnPokeZoneEnter(HoverEnterEventArgs args) => WarnAboutPoke(true);


    /// <summary>
    /// Handles exit from poke zone. Stops poke warning.
    /// </summary>
    /// <param name="args"></param>
    private void OnPokeZoneExit(UIHoverEventArgs args) => WarnAboutPoke(false);

    /// <summary>
    /// Handles exit from poke zone. Stops poke warning.
    /// </summary>
    /// <param name="args"></param>
    private void OnPokeZoneExit(HoverExitEventArgs args) => WarnAboutPoke(false);


    /// <summary>
    /// Handles poke start. Shows poke.
    /// </summary>
    /// <param name="context"></param>
    private void OnPokeStart(InputAction.CallbackContext context) => ShowPoke(true);

    /// <summary>
    /// Handles poke stop. Stops showing poke
    /// </summary>
    /// <param name="context"></param>
    private void OnPokeStop(InputAction.CallbackContext context) => ShowPoke(false);


    /// <summary>
    /// Handles entrance to grab zone. Warns about grab.
    /// </summary>
    /// <param name="args"></param>
    private void OnGrabZoneEnter(HoverEnterEventArgs args) => WarnAboutGrab(true);

    /// <summary>
    /// Handles exit from grab zone. Stops grab waring.
    /// </summary>
    /// <param name="args"></param>
    private void OnGrabZoneExit(HoverExitEventArgs args) => WarnAboutGrab(false);


    /// <summary>
    /// Handles grab start. Shows grab.
    /// </summary>
    /// <param name="args"></param>
    private void OnGrabStart(SelectEnterEventArgs args) => ShowGrab(true);

    /// <summary>
    /// Handles grab stop. Stops showing grab.
    /// </summary>
    /// <param name="args"></param>
    private void OnGrabStop(SelectExitEventArgs args) => ShowGrab(false);


    /// <summary>
    /// Handles recording start. Shows recording.
    /// </summary>
    private void OnRecordingStart() => ShowRecording(true);

    /// <summary>
    /// Handles recording stop. Stops showing recording.
    /// </summary>
    private void OnRecordingStop() => ShowRecording(false);


    /// <summary>
    /// Handles entrance of teleportation destination to available zone. Shows teleport action.
    /// </summary>
    /// <param name="args"></param>
    private void OnTeleportStart(SelectEnterEventArgs args) => ShowTeleport(true);

    /// <summary>
    /// Handles exit of teleportation destination from available zone. Stops showing teleport action.
    /// </summary>
    /// <param name="args"></param>
    private void OnTeleportStop(SelectExitEventArgs args) => ShowTeleport(false);


    /// <summary>
    /// Handles pause start. Warns about play/pause.
    /// </summary>
    private void OnPauseStart() => WarnAboutPause(true);

    /// <summary>
    /// Handles pause stop. Stops play/pause warning.
    /// </summary>
    private void OnPauseStop() => WarnAboutPause(false);

    protected virtual void OnDisable()
    {
        _grabInteractor?.hoverEntered.RemoveListener(OnGrabZoneEnter);
        _grabInteractor?.hoverExited.RemoveListener(OnGrabZoneExit);

        _grabInteractor?.selectEntered.RemoveListener(OnGrabStart);
        _grabInteractor?.selectExited.RemoveListener(OnGrabStop);

        if (_uiInteractor)
        {
            _uiInteractor.uiHoverEntered.RemoveListener(OnPokeZoneEnter);
            _uiInteractor.uiHoverExited.RemoveListener(OnPokeZoneExit);

            _uiInteractor.uiPressInput.inputActionReferencePerformed.action.performed -= OnPokeStart;
            _uiInteractor.uiPressInput.inputActionReferencePerformed.action.canceled -= OnPokeStop;
        }

        _uiPokeInteractor?.uiHoverEntered.RemoveListener(OnPokeZoneEnter);
        _uiPokeInteractor?.uiHoverExited.RemoveListener(OnPokeZoneExit);

        _pokeInteractor?.hoverEntered.RemoveListener(OnPokeZoneEnter);
        _pokeInteractor?.hoverExited.RemoveListener(OnPokeZoneExit);

        _teleportInteractor?.selectEntered.RemoveListener(OnTeleportStart);
        _teleportInteractor?.selectExited.RemoveListener(OnTeleportStop);

        if(_speechController)
        {
            _speechController.onRecordingStart -= OnRecordingStart;
            _speechController.onRecordingStop -= OnRecordingStop;
        }
        
        Pause.onPause -= OnPauseStart;
        Pause.onContinue -= OnPauseStop;
    }
}
