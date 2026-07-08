using UnityEngine;

/// <summary>
/// Controll the book when it's grabbed by the player.
/// If the book is in camera visibility zone, it changes state to opened.
/// </summary>
public class DictionaryGrabState : DictionaryState
{
    public DictionaryGrabState(DictionaryBook book, DictionaryStateMachine stateMachine) : base(book, stateMachine) { }

    /// <summary>
    /// Cancels book returning if it was started.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        _book.StopReturing();
    }

    /// <summary>
    /// Checks if the book is in camera visibility zone.
    /// </summary>
    public override void Update()
    {
        if (_book.IsInFrontOfCamera())
            _stateMachine.ChangeState(_stateMachine.OpenState);
    }
}
