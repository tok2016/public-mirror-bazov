using UnityEngine;

/// <summary>
/// Represents button's behaviour when it shows that it can be used.
/// </summary>
public class MetaButtonWarnState : MetaState
{
    public MetaButtonWarnState(MetaButton metaButton, Color color, MetaStateMachine stateMachine) 
        : base(metaButton, color, stateMachine) { }

    /// <summary>
    /// Changes state to disabled or pressed. Returns to paused or default if button is no more warned.
    /// </summary>
    public override void Update()
    {
        if (_stateMachine.IsDisabled)
            _stateMachine.ChangeState(_stateMachine.DisabledState);
        else if (_stateMachine.IsPressed)
            _stateMachine.ChangeState(_stateMachine.PressState);
        else if (!_stateMachine.IsWarned)
        {
            if (_stateMachine.IsPaused)
                _stateMachine.ChangeState(_stateMachine.PauseState);
            else
                _stateMachine.ChangeState(_stateMachine.DefaultState);
        }
    }
}
