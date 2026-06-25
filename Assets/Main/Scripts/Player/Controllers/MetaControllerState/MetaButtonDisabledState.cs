using UnityEngine;

public class MetaButtonDisabledState : MetaState
{
    public MetaButtonDisabledState(MetaButton metaButton, Color color, MetaStateMachine stateMachine) 
        : base(metaButton, color, stateMachine) { }

    public override void Update()
    {
        if (_stateMachine.IsWarned)
            _stateMachine.ChangeState(_stateMachine.WarnState);
        else if (_stateMachine.IsPaused)
            _stateMachine.ChangeState(_stateMachine.PauseState);
        else if (!_stateMachine.IsDisabled)
            _stateMachine.ChangeState(_stateMachine.DefaultState);
    }
}
