using UnityEngine;

/// <summary>
/// Represents button's behaviour when it's pressed.
/// </summary>
public class MetaButtonPressState : MetaState
{
    public MetaButtonPressState(MetaButton metaButton, Color color, MetaStateMachine stateMachine) 
        : base(metaButton, color, stateMachine) { }

    /// <summary>
    /// Changes state to disabled or paused. Returns to warned or default if button is no more pressed.
    /// </summary>
    public override void Update()
    {
        if (_stateMachine.IsDisabled)
            _stateMachine.ChangeState(_stateMachine.DisabledState);
        else if (_stateMachine.IsPaused)
            _stateMachine.ChangeState(_stateMachine.PauseState);
        else if (!_stateMachine.IsPressed)
        {
            if (_stateMachine.IsWarned)
                _stateMachine.ChangeState(_stateMachine.WarnState);
            else
                _stateMachine.ChangeState(_stateMachine.DefaultState);
        }
    }
}
