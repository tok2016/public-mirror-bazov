using UnityEngine;
using UnityEngine.InputSystem;

public enum HintButtonState
{
    Default = 0,
    Pause = 1,
    Warn = 2,
    Pressed = 3,
    Disabled = 4
}

public class HintButton : HintObject
{
    [SerializeField] private InputActionReference _action;
    [SerializeField] private HintButtionProps _hintButtionProps;
    [SerializeField] private GameObject _uiHint;

    private HintButtonState _state, _prevState;
    private bool _isPaused, _isWarned;

    public HintButtonState State
    {
        get => _state;
        private set
        {
            if (!_renderer) return;
            _state = value;
            _renderer.material = _hintButtionProps.StateMaterials[_state];
        }
    }

    protected override void Awake()
    {
        base.Awake();

        State = HintButtonState.Default;
        _uiHint.SetActive(false);
    }

    public override void ToggleMaterial(bool enable)
    {
        State = enable ? HintButtonState.Warn : HintButtonState.Default;

        if (!enable)
        {
            _isWarned = false;
            _isPaused = false;
        }
    }

    public void Return()
    {
        State = _prevState;
    }

    public void Warn(bool enable)
    {
        _isWarned = enable;
        _prevState = _isPaused ? HintButtonState.Pause : HintButtonState.Default;

        if (enable)
            State = HintButtonState.Warn;
        else
            Return();
    }

    public void Pause(bool enable)
    {
        _isPaused = enable;
        _uiHint.SetActive(enable);
        _prevState = _isWarned ? HintButtonState.Warn : HintButtonState.Default;
        if(enable)
            State = HintButtonState.Pause;
        else 
            Return();
    }

    public void Press()
    {
        if (State < HintButtonState.Pause) return;

        _prevState = _isWarned ? HintButtonState.Warn : HintButtonState.Pause;
        State = HintButtonState.Pressed;
    }

    public void Disable()
    {
        if (State < HintButtonState.Pause) return;

        _prevState = _isWarned ? HintButtonState.Warn : HintButtonState.Pause;
        State = HintButtonState.Disabled;
    }
}
