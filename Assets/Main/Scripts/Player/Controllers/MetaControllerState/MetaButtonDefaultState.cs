using UnityEngine;

/// <summary>
/// Represents button's default behaviour.
/// </summary>
public class MetaButtonDefaultState : MetaState
{
    public MetaButtonDefaultState(MetaButton metaButton, Color color, MetaStateMachine stateMachine) 
        : base(metaButton, color, stateMachine) { }

    /// <summary>
    /// Changes state when button is warned or paused.
    /// </summary>
    public override void Update()
    {
        if (_stateMachine.IsPaused)
            _stateMachine.ChangeState(_stateMachine.PauseState);
        else if (_stateMachine.IsWarned)
            _stateMachine.ChangeState(_stateMachine.WarnState);
    }
}
