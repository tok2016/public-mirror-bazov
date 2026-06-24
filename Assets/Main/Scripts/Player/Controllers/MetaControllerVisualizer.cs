using UnityEngine;

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

    public override void DisableGrab(bool enable)
    {
        _grabButton?.Disable(enable);
    }

    public override void DisablePause(bool enable)
    {
        _pauseButton?.Disable(enable);
    }

    public override void DisablePoke(bool enable)
    {
        _pokeButton?.Disable(enable);
    }

    public override void DisableRecording(bool enable)
    {
        _recordingButton?.Disable(enable);
    }

    public override void DisableTeleport(bool enable)
    {
        _teleportButton?.Disable(enable);
    }

    public override void ShowGrab(bool enable)
    {
        _grabButton?.Press(enable);
    }

    public override void ShowPause(bool enable)
    {
        _pauseButton?.Press(enable);
    }

    public override void ShowPoke(bool enable)
    {
        _pokeButton?.Press(enable);
    }

    public override void ShowRecording(bool enable)
    {
        _recordingButton?.Press(enable);
    }

    public override void ShowTeleport(bool enable)
    {
        _teleportButton?.Press(enable);
    }

    public override void WarnAboutGrab(bool enable)
    {
        _grabButton?.Warn(enable);
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

        _grabButton?.Pause(enable);
        _pokeButton?.Pause(enable);
        _recordingButton?.Pause(enable);
        _teleportButton?.Pause(enable);
        _pauseButton?.Pause(enable);
    }

    public override void WarnAboutPoke(bool enable)
    {
        _pokeButton?.Warn(enable);
    }

    public override void WarnAboutRecording(bool enable)
    {
        _recordingButton?.Warn(enable);
    }

    public override void WarnAboutTeleport(bool enable)
    {
        _teleportButton?.Warn(enable);
    }
}
