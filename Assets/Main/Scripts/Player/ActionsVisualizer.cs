using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;

[Serializable]
public struct VisualizerMaterialSetting
{
    public Renderer renderer;
    public Material visibleMaterial;
    public Material translucentMaterial;
}

public abstract class ActionsVisualizer : MonoBehaviour
{
    [SerializeField] protected XRBaseInteractor _grabInteractor, _pokeInteractor, _teleportInteractor;
    [SerializeField] protected NearFarInteractor _uiInteractor;
    [SerializeField] protected SpeechController _speechController;

    [SerializeField] protected VisualizerMaterialSetting[] _materials;

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

        _pokeInteractor?.hoverEntered.AddListener(OnPokeZoneEnter);
        _pokeInteractor?.hoverExited.AddListener(OnPokeZoneExit);

        _teleportInteractor?.selectEntered.AddListener(OnTeleportStart);
        _teleportInteractor?.selectExited.AddListener(OnTeleportStop);

        if (_speechController)
        {
            _speechController.onRecordingStart += OnRecordingStart;
            _speechController.onRecordingStop += OnRecordingStop;
        }
    }

    public abstract void WarnAboutPoke(bool enable);
    public abstract void ShowPoke(bool enable);
    public abstract void DisablePoke(bool enable);

    public abstract void WarnAboutGrab(bool enable);
    public abstract void ShowGrab(bool enable);
    public abstract void DisableGrab(bool enable);

    public abstract void WarnAboutRecording(bool enable);
    public abstract void ShowRecording(bool enable);
    public abstract void DisableRecording(bool enable);

    public abstract void WarnAboutTeleport(bool enable);
    public abstract void ShowTeleport(bool enable);
    public abstract void DisableTeleport(bool enable);

    public void SetInvisible()
    {
        gameObject.SetActive(false);
    }

    public void SetTranslucent()
    {
        gameObject.SetActive(true);
        foreach (var material in _materials)
            material.renderer.material = material.translucentMaterial;
    }

    public void SetVisible()
    {
        gameObject.SetActive(true);
        foreach (var material in _materials)
            material.renderer.material = material.visibleMaterial;
    }

    private void OnPokeZoneEnter(UIHoverEventArgs args) => WarnAboutPoke(true);
    private void OnPokeZoneEnter(HoverEnterEventArgs args) => WarnAboutPoke(true);

    private void OnPokeZoneExit(UIHoverEventArgs args) => WarnAboutPoke(false);
    private void OnPokeZoneExit(HoverExitEventArgs args) => WarnAboutPoke(false);

    private void OnPokeStart(InputAction.CallbackContext context) => ShowPoke(true);
    private void OnPokeStop(InputAction.CallbackContext context) => ShowPoke(false);

    private void OnGrabZoneEnter(HoverEnterEventArgs args) => WarnAboutGrab(true);
    private void OnGrabZoneExit(HoverExitEventArgs args) => WarnAboutGrab(false);

    private void OnGrabStart(SelectEnterEventArgs args) => ShowGrab(true);
    private void OnGrabStop(SelectExitEventArgs args) 
    {
        ShowGrab(false);
        WarnAboutGrab(false);
    }

    private void OnRecordingStart() => ShowRecording(true);
    private void OnRecordingStop() => ShowRecording(false);

    private void OnTeleportStart(SelectEnterEventArgs args) => ShowTeleport(true);
    private void OnTeleportStop(SelectExitEventArgs args) => ShowTeleport(false);

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

        _pokeInteractor?.hoverEntered.RemoveListener(OnPokeZoneEnter);
        _pokeInteractor?.hoverExited.RemoveListener(OnPokeZoneExit);

        _teleportInteractor?.selectEntered.RemoveListener(OnTeleportStart);
        _teleportInteractor?.selectExited.RemoveListener(OnTeleportStop);

        if(_speechController)
        {
            _speechController.onRecordingStart -= OnRecordingStart;
            _speechController.onRecordingStop -= OnRecordingStop;
        }
    }
}
