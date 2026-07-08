using UnityEngine;

/// <summary>
/// Visualizes input actions by highlighting Meta controller's buttons.
/// </summary>
public class MetaControllerVisualizer : ActionsVisualizer
{
    [Header("Render")]
    [SerializeField] private Renderer _metaControllerRenderer;
    private Material _metaControllerMaterial;

    private VisualizerState _prevState;

    [Header("Buttons")]
    [SerializeField] private MetaButtonProps _grabProps;
    [SerializeField] private MetaButtonProps _pokeProps, _teleportProps, _recordingProps, _pauseProps;

    private MetaButton _grabButton, _pokeButton, _teleportButton, _recordingButton, _pauseButton;

    private void Awake()
    {
        _metaControllerMaterial = _metaControllerRenderer.material;

        if(_grabProps)
            _grabButton = new MetaButton(_grabProps, _metaControllerMaterial);
        if(_pokeProps)
            _pokeButton = new MetaButton(_pokeProps, _metaControllerMaterial);
        if(_teleportProps)
            _teleportButton = new MetaButton(_teleportProps, _metaControllerMaterial);
        if(_recordingProps)
            _recordingButton = new MetaButton(_recordingProps, _metaControllerMaterial);
        if(_pauseProps)
            _pauseButton = new MetaButton(_pauseProps, _metaControllerMaterial);
    }

    private void Update()
    {
        _grabButton?.Update();
        _pokeButton?.Update();
        _teleportButton?.Update();
        _recordingButton?.Update();
        _pauseButton?.Update();
    }


    /// <summary>
    /// Highlights grab button as disabled or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights grab button as disabled.</param>
    public override void DisableGrab(bool enable)
    {
        _grabButton?.Disable(enable);
    }

    /// <summary>
    /// Highlights pause button as disabled or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights pause button as disabled.</param>
    public override void DisablePause(bool enable)
    {
        _pauseButton?.Disable(enable);
    }

    /// <summary>
    /// Highlights poke button as disabled or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights poke button as disabled.</param>
    public override void DisablePoke(bool enable)
    {
        _pokeButton?.Disable(enable);
    }

    /// <summary>
    /// Highlights record button as disabled or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights record button as disabled.</param>
    public override void DisableRecording(bool enable)
    {
        _recordingButton?.Disable(enable);
    }

    /// <summary>
    /// Highlights teleport button as disabled or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights teleport button as disabled.</param>
    public override void DisableTeleport(bool enable)
    {
        _teleportButton?.Disable(enable);
    }


    /// <summary>
    /// Highlights grab button as pressed or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights grab button as pressed.</param>
    public override void ShowGrab(bool enable)
    {
        _grabButton?.Press(enable);
    }

    /// <summary>
    /// Highlights pause button as pressed or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights pause button as pressed.</param>
    public override void ShowPause(bool enable)
    {
        _pauseButton?.Press(enable);
    }

    /// <summary>
    /// Highlights poke button as pressed or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights poke button as pressed.</param>
    public override void ShowPoke(bool enable)
    {
        _pokeButton?.Press(enable);
    }

    /// <summary>
    /// Highlights record button as pressed or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights record button as pressed.</param>
    public override void ShowRecording(bool enable)
    {
        _recordingButton?.Press(enable);
    }

    /// <summary>
    /// Highlights teleport button as pressed or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights teleport button as pressed.</param>
    public override void ShowTeleport(bool enable)
    {
        _teleportButton?.Press(enable);
    }


    /// <summary>
    /// Highlights grab button as warned or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights grab button as warned.</param>
    public override void WarnAboutGrab(bool enable)
    {
        _grabButton?.Warn(enable);
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

        _grabButton?.Pause(enable);
        _pokeButton?.Pause(enable);
        _recordingButton?.Pause(enable);
        _teleportButton?.Pause(enable);
        _pauseButton?.Pause(enable);
    }

    /// <summary>
    /// Highlights poke button as warned or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights poke button as warned.</param>
    public override void WarnAboutPoke(bool enable)
    {
        _pokeButton?.Warn(enable);
    }

    /// <summary>
    /// Highlights record button as warned or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights record button as warned.</param>
    public override void WarnAboutRecording(bool enable)
    {
        _recordingButton?.Warn(enable);
    }

    /// <summary>
    /// Highlights teleport button as warned or resets it.
    /// </summary>
    /// <param name="enable">If true, highlights teleport button as warned.</param>
    public override void WarnAboutTeleport(bool enable)
    {
        _teleportButton?.Warn(enable);
    }
}
