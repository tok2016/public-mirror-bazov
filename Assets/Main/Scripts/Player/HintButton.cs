using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Ordered states of hint button.
/// Default - hint is disabled.
/// Pause - shows icon on button.
/// Warn - warns player that button can be used at this moment.
/// Pressed - shows that button is currently used.
/// Disabled - shows that button won't trigger.
/// </summary>
public enum HintButtonState
{
    Default = 0,
    Pause = 1,
    Warn = 2,
    Pressed = 3,
    Disabled = 4
}

/// <summary>
/// Highlights button with different referring to its state.
/// </summary>
public class HintButton : HintObject
{
    [SerializeField] private InputActionReference _action;
    [SerializeField] private HintButtionProps _hintButtionProps;
    [SerializeField] private GameObject _uiHint;

    private HintButtonState _state, _prevState;
    private bool _isPaused, _isWarned;

    /// <summary>
    /// Changes current state and material of button.
    /// </summary>
    /// <value>
    /// Current state of button. 
    /// </value>
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


    /// <summary>
    /// Changes state to Warn or resets it to Default.
    /// </summary>
    /// <param name="enable">If enabled, changes state to Warn.</param>
    public override void ToggleMaterial(bool enable)
    {
        State = enable ? HintButtonState.Warn : HintButtonState.Default;

        if (!enable)
        {
            _isWarned = false;
            _isPaused = false;
        }
    }

    /// <summary>
    /// Returns to previously stored state.
    /// </summary>
    public void Return()
    {
        State = _prevState;
    }

    /// <summary>
    /// Changes state to Warn or previous.
    /// </summary>
    /// <param name="enable">If enabled, changes state to Warn.</param>
    public void Warn(bool enable)
    {
        _isWarned = enable;
        _prevState = _isPaused ? HintButtonState.Pause : HintButtonState.Default;

        if (enable)
            State = HintButtonState.Warn;
        else
            Return();
    }

    /// <summary>
    /// Changes state to Pause or previous.
    /// </summary>
    /// <param name="enable">If enabled, changes state to Pause.</param>
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

    /// <summary>
    /// Changes state to Pressed.
    /// </summary>
    public void Press()
    {
        if (State < HintButtonState.Pause) return;

        _prevState = _isWarned ? HintButtonState.Warn : HintButtonState.Pause;
        State = HintButtonState.Pressed;
    }

    /// <summary>
    /// Changes state to Disabled.
    /// </summary>
    public void Disable()
    {
        if (State < HintButtonState.Pause) return;

        _prevState = _isWarned ? HintButtonState.Warn : HintButtonState.Pause;
        State = HintButtonState.Disabled;
    }
}
