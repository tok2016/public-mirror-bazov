using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class HandController : MonoBehaviour
{
    [SerializeField] private NearFarInteractor _nearFarInteractor;
    [SerializeField] private XRPokeInteractor _pokeInteractor;
    [SerializeField] private XRRayInteractor _teleportInteractor;
    [SerializeField] private SpeechController _speechController;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _nearFarInteractor.hoverEntered.AddListener(SetReadyToGrabPose);
        _nearFarInteractor.hoverExited.AddListener(ResetReadyToGrabPose);

        _nearFarInteractor.selectEntered.AddListener(SetGrabPose);
        _nearFarInteractor.selectExited.AddListener(ResetGrabPose);

        _nearFarInteractor.uiHoverEntered.AddListener(SetPokePose);
        _nearFarInteractor.uiHoverExited.AddListener(ResetPokePose);

        _nearFarInteractor.uiPressInput.inputActionReferencePerformed.action.performed += Poke;

        _pokeInteractor.hoverEntered.AddListener(SetPokePose);
        _pokeInteractor.hoverExited.AddListener(ResetPokePose);

        _teleportInteractor.hoverEntered.AddListener(SetPokePose);
        _teleportInteractor.hoverExited.AddListener(ResetPokePose);

        _speechController.onRecordingStart += SetRecoringPose;
        _speechController.onRecordingStop += ResetRecordingPose;
    }

    public void SetPoking(bool enable)
    {
        _animator.SetBool("IsPoking", enable);
    }

    public void GetReadyToGrab(bool enable)
    {
        _animator.SetBool("ReadyToGrab", enable);
    }

    public void SetGrabbing(bool enable)
    {
        _animator.SetBool("IsGrabbing", enable);
    }

    public void GetReadyToRecord(bool enable)
    {
        _animator.SetBool("ReadyToRecord", enable);
    }

    public void SetRecording(bool enable)
    {
        _animator.SetBool("IsRecording", enable);
    }

    private void SetReadyToGrabPose(HoverEnterEventArgs args) => GetReadyToGrab(true);
    private void ResetReadyToGrabPose(HoverExitEventArgs args) => GetReadyToGrab(false);

    private void SetGrabPose(SelectEnterEventArgs args) => SetGrabbing(true);
    private void ResetGrabPose(SelectExitEventArgs args) => SetGrabbing(false);

    private void SetPokePose(UIHoverEventArgs args) => SetPoking(true);
    private void SetPokePose(HoverEnterEventArgs args) => SetPoking(true);

    private void ResetPokePose(UIHoverEventArgs args) => SetPoking(false);
    private void ResetPokePose(HoverExitEventArgs args) => SetPoking(false);

    public void Poke()
    {
        _animator.SetTrigger("Poke");
    }

    private void Poke(InputAction.CallbackContext context) => Poke();

    private void SetRecoringPose() => SetRecording(true);
    private void ResetRecordingPose() => SetRecording(false);

    private void OnDisable()
    {
        _nearFarInteractor.hoverEntered.RemoveListener(SetReadyToGrabPose);
        _nearFarInteractor.hoverExited.RemoveListener(ResetReadyToGrabPose);

        _nearFarInteractor.selectEntered.RemoveListener(SetGrabPose);
        _nearFarInteractor.selectExited.RemoveListener(ResetGrabPose);

        _nearFarInteractor.uiHoverEntered.RemoveListener(SetPokePose);
        _nearFarInteractor.uiHoverExited.RemoveListener(ResetPokePose);

        _nearFarInteractor.uiPressInput.inputActionReferencePerformed.action.performed -= Poke;

        _pokeInteractor.hoverEntered.RemoveListener(SetPokePose);
        _pokeInteractor.hoverExited.RemoveListener(ResetPokePose);

        _teleportInteractor.hoverEntered.RemoveListener(SetPokePose);
        _teleportInteractor.hoverExited.RemoveListener(ResetPokePose);

        _speechController.onRecordingStart -= SetRecoringPose;
        _speechController.onRecordingStop -= ResetRecordingPose;
    }
}
