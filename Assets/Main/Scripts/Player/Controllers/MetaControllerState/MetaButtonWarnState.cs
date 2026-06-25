using UnityEngine;

public class MetaButtonWarnState : MetaState
{
    public MetaButtonWarnState(MetaButton metaButton, Color color, MetaStateMachine stateMachine) 
        : base(metaButton, color, stateMachine) { }

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
