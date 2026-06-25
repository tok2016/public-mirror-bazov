using UnityEngine;

public class MetaButtonPauseState : MetaState
{
    public MetaButtonPauseState(MetaButton metaButton, Color color, MetaStateMachine stateMachine) 
        : base(metaButton, color, stateMachine) { }

    public override void Update()
    {
        if (_stateMachine.IsDisabled)
            _stateMachine.ChangeState(_stateMachine.DisabledState);
        else if (_stateMachine.IsWarned)
            _stateMachine.ChangeState(_stateMachine.WarnState);
        else if(!_stateMachine.IsPaused)
            _stateMachine.ChangeState(_stateMachine.DefaultState);
    }
}
