using UnityEngine;

public class ControllerVisualizer : ActionsVisualizer
{
    [Header("Buttons")]
    [SerializeField] private HintButton _grabButton;
    [SerializeField] private HintButton _pokeButton, _teleportationStick, _recordingButton, _pauseButton;
    private VisualizerState _prevState;

    protected override void OnDisable()
    {
        base.OnDisable();
        _grabButton?.ToggleMaterial(false);
        _pokeButton?.ToggleMaterial(false);
        _teleportationStick?.ToggleMaterial(false);
        _recordingButton?.ToggleMaterial(false);
        _pauseButton?.ToggleMaterial(false);
    }

    public override void DisableGrab(bool enable)
    {
        DisableButton(_grabButton, enable);
    }

    public override void DisablePoke(bool enable)
    {
        DisableButton(_pokeButton, enable);
    }

    public override void DisableRecording(bool enable)
    {
        DisableButton(_recordingButton, enable);
    }

    public override void DisableTeleport(bool enable)
    {
        DisableButton(_teleportationStick, enable);
    }

    private void DisableButton(HintButton button, bool enable)
    {
        if (enable)
            button?.Disable();
        else
            button?.Return();
    }

    public override void ShowGrab(bool enable)
    {
        ShowButton(_grabButton, enable);
    }

    public override void ShowPoke(bool enable)
    {
        ShowButton(_pokeButton, enable);
    }

    public override void ShowRecording(bool enable)
    {
        ShowButton(_recordingButton, enable);
    }

    public override void ShowTeleport(bool enable)
    {
        ShowButton(_teleportationStick, enable);
    }

    private void ShowButton(HintButton button, bool enable)
    {
        if (enable)
            button?.Press();
        else
            button?.Return();
    }

    public override void WarnAboutGrab(bool enable)
    {
        WarnAboutButton(_grabButton, enable);
    }

    public override void WarnAboutPoke(bool enable)
    {
        WarnAboutButton(_pokeButton, enable);
    }

    public override void WarnAboutRecording(bool enable)
    {
        WarnAboutButton(_recordingButton, enable);
    }

    public override void WarnAboutTeleport(bool enable)
    {
        WarnAboutButton(_teleportationStick, enable);
    }

    public void WarnAboutAllActions(bool enable)
    {
        PauseButton(_grabButton, enable);
        PauseButton(_pokeButton, enable);
        PauseButton(_recordingButton, enable);
        PauseButton(_teleportationStick, enable);
        PauseButton(_pauseButton, enable);
    }

    public override void WarnAboutPause(bool enable)
    {
        if (enable)
        {
            _prevState = State;
            State = VisualizerState.Visible;
        }
        else
            State = _prevState;

        WarnAboutAllActions(enable);
    }

    private void WarnAboutButton(HintButton button, bool enable)
    {
        button?.Warn(enable);
    }

    private void PauseButton(HintButton button, bool enable)
    {
        button?.Pause(enable);
    }

    public override void ShowPause(bool enable)
    {
        ShowButton(_pauseButton, enable);
    }

    public override void DisablePause(bool enable)
    {
        DisableButton(_pauseButton, enable);
    }
}
