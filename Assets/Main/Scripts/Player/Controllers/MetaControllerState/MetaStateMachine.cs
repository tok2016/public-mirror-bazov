using UnityEngine;

public class MetaStateMachine
{
    public bool IsWarned { get; set; }
    public bool IsPaused { get; set; }
    public bool IsPressed { get; set; }
    public bool IsDisabled {  get; set; }

    public MetaButtonDefaultState DefaultState { get; private set; }
    public MetaButtonWarnState WarnState { get; private set; }
    public MetaButtonPauseState PauseState { get; private set; }
    public MetaButtonPressState PressState { get; private set; }
    public MetaButtonDisabledState DisabledState { get; private set; }

    private MetaState _current;

    public MetaStateMachine(MetaButton metaButton, MetaButtonProps props)
    {
        DefaultState = new MetaButtonDefaultState(metaButton, props.DefaultColor, this);
        WarnState = new MetaButtonWarnState(metaButton, props.WarningColor, this);
        PauseState = new MetaButtonPauseState(metaButton, props.PauseColor, this);
        PressState = new MetaButtonPressState(metaButton, props.PressColor, this);
        DisabledState = new MetaButtonDisabledState(metaButton, props.DisableColor, this);

        _current = DefaultState;
        _current.Enter();
    }

    public void ChangeState(MetaState nextState)
    {
        _current?.Exit();
        _current = nextState;
        _current?.Enter();
    }

    public void Update()
    {
        _current?.Update();
    }
}
