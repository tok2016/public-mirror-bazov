using UnityEngine;

public class DictionaryGrabState : DictionaryState
{
    public DictionaryGrabState(DictionaryBook book, DictionaryStateMachine stateMachine) : base(book, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _book.StopReturing();
    }

    public override void Update()
    {
        if (_book.IsInFrontOfCamera())
            _stateMachine.ChangeState(_stateMachine.OpenState);
    }
}
