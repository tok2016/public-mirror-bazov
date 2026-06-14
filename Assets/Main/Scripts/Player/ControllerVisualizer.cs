using UnityEngine;

public class ControllerVisualizer : ActionsVisualizer
{
    [Header("Buttons")]
    [SerializeField] private HintButton _grabButton;
    [SerializeField] private HintButton _pokeButton, _teleportationStick, _recordingButton;

    public override void DisableGrab(bool enable)
    {
        _grabButton?.ToggleDisable(enable);
    }

    public override void DisablePoke(bool enable)
    {
        _pokeButton?.ToggleDisable(enable);
    }

    public override void DisableRecording(bool enable)
    {
        _recordingButton?.ToggleDisable(enable);
    }

    public override void DisableTeleport(bool enable)
    {
        _teleportationStick?.ToggleDisable(enable);
    }

    public override void ShowGrab(bool enable)
    {
        _grabButton?.TogglePressHint(enable);
    }

    public override void ShowPoke(bool enable)
    {
        _pokeButton?.TogglePressHint(enable);
    }

    public override void ShowRecording(bool enable)
    {
        _recordingButton?.TogglePressHint(enable);
    }

    public override void ShowTeleport(bool enable)
    {
        _teleportationStick?.TogglePressHint(enable);
    }

    public override void WarnAboutGrab(bool enable)
    {
        _grabButton?.ToggleMaterial(enable);
    }

    public override void WarnAboutPoke(bool enable)
    {
        _pokeButton?.ToggleMaterial(enable);
    }

    public override void WarnAboutRecording(bool enable)
    {
        _recordingButton?.ToggleMaterial(enable);
    }

    public override void WarnAboutTeleport(bool enable)
    {
        _teleportationStick?.ToggleMaterial(enable);
    }
}
