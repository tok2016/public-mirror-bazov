using UnityEngine;

/// <summary>
/// Visualizes input actions by highlighting default XRI controller's buttons.
/// </summary>
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

    /// <summary>
    /// Highlights grab button as disabled or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights grab button as disabled.</param>
    public override void DisableGrab(bool enable)
    {
        DisableButton(_grabButton, enable);
    }

    /// <summary>
    /// Highlights poke button as disabled or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights poke button as disabled.</param>
    public override void DisablePoke(bool enable)
    {
        DisableButton(_pokeButton, enable);
    }

    /// <summary>
    /// Highlights record button as disabled or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights record button as disabled.</param>
    public override void DisableRecording(bool enable)
    {
        DisableButton(_recordingButton, enable);
    }

    /// <summary>
    /// Highlights teleport button as disabled or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights teleport button as disabled.</param>
    public override void DisableTeleport(bool enable)
    {
        DisableButton(_teleportationStick, enable);
    }

    /// <summary>
    /// Highlights given button as disabled or resets it.
    /// </summary>
    /// <param name="button">Hint button to highlight.</param>
    /// <param name="enable">If true, highlights given button as disabled.</param>
    private void DisableButton(HintButton button, bool enable)
    {
        if (enable)
            button?.Disable();
        else
            button?.Return();
    }


    /// <summary>
    /// Highlights grab button as pressed or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights grab button as pressed.</param>
    public override void ShowGrab(bool enable)
    {
        ShowButton(_grabButton, enable);
    }

    /// <summary>
    /// Highlights poke button as pressed or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights poke button as pressed.</param>
    public override void ShowPoke(bool enable)
    {
        ShowButton(_pokeButton, enable);
    }

    /// <summary>
    /// Highlights record button as pressed or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights record button as pressed.</param>
    public override void ShowRecording(bool enable)
    {
        ShowButton(_recordingButton, enable);
    }

    /// <summary>
    /// Highlights teleport button as pressed or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights teleport button as pressed.</param>
    public override void ShowTeleport(bool enable)
    {
        ShowButton(_teleportationStick, enable);
    }

    /// <summary>
    /// Highlights given button as pressed or resets it.
    /// </summary>
    /// <param name="button">Hint button to highlight.</param>
    /// <param name="enable">If true, highlights given button as pressed.</param>
    private void ShowButton(HintButton button, bool enable)
    {
        if (enable)
            button?.Press();
        else
            button?.Return();
    }


    /// <summary>
    /// Highlights grab button as warned or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights grab button as warned.</param>
    public override void WarnAboutGrab(bool enable)
    {
        WarnAboutButton(_grabButton, enable);
    }

    /// <summary>
    /// Highlights poke button as warned or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights poke button as warned.</param>
    public override void WarnAboutPoke(bool enable)
    {
        WarnAboutButton(_pokeButton, enable);
    }

    /// <summary>
    /// Highlights record button as warned or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights record button as warned.</param>
    public override void WarnAboutRecording(bool enable)
    {
        WarnAboutButton(_recordingButton, enable);
    }

    /// <summary>
    /// Highlights teleport button as warned or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights teleport button as warned.</param>
    public override void WarnAboutTeleport(bool enable)
    {
        WarnAboutButton(_teleportationStick, enable);
    }

    /// <summary>
    /// Highlights all hint buttons as paused or resets them.
    /// </summary>
    /// <param name="enable">If true, highlights all buttons as paused.</param>
    private void WarnAboutAllActionsInPause(bool enable)
    {
        PauseButton(_grabButton, enable);
        PauseButton(_pokeButton, enable);
        PauseButton(_recordingButton, enable);
        PauseButton(_teleportationStick, enable);
        PauseButton(_pauseButton, enable);
    }

    /// <summary>
    /// Makes controller visible and highlights all buttons as paused or resets them.
    /// </summary>
    /// <param name="enable">If true, makes controller visible and highlights all buttons as paused.</param>
    public override void WarnAboutPause(bool enable)
    {
        if (enable)
        {
            _prevState = State;
            State = VisualizerState.Visible;
        }
        else
            State = _prevState;

        WarnAboutAllActionsInPause(enable);
    }

    /// <summary>
    /// Highlights given button as warned or resets it.
    /// </summary>
    /// <param name="button">Hint button to highlight.</param>
    /// <param name="enable">If true, highlights given button as warned.</param>
    private void WarnAboutButton(HintButton button, bool enable)
    {
        button?.Warn(enable);
    }


    /// <summary>
    /// Highlights given button as paused ot resets it.
    /// </summary>
    /// <param name="button">Hint button to highlight.</param>
    /// <param name="enable">If true, highlights given button as paused.</param>
    private void PauseButton(HintButton button, bool enable)
    {
        button?.Pause(enable);
    }

    /// <summary>
    /// Highlights pause button as pressed or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights pause button as pressed.</param>
    public override void ShowPause(bool enable)
    {
        ShowButton(_pauseButton, enable);
    }

    /// <summary>
    /// Highlights pause button as disabled or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights pause button as disabled.</param>
    public override void DisablePause(bool enable)
    {
        DisableButton(_pauseButton, enable);
    }
}
