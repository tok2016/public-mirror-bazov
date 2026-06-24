using UnityEngine;

public class MetaButtonDefaultState : MetaState
{
    public MetaButtonDefaultState(MetaButton metaButton, Color color, MetaStateMachine stateMachine) 
        : base(metaButton, color, stateMachine) { }

    public override void Update()
    {
        if (_stateMachine.IsPaused)
            _stateMachine.ChangeState(_stateMachine.PauseState);
        else if (_stateMachine.IsWarned)
            _stateMachine.ChangeState(_stateMachine.WarnState);
    }
}
