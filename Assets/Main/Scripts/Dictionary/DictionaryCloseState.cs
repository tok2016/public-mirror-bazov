using UnityEngine;

/// <summary>
/// Controlls the book when it's closed. Does nothing.
/// </summary>
public class DictionaryCloseState : DictionaryState
{
    public DictionaryCloseState(DictionaryBook book, DictionaryStateMachine stateMachine) : base(book, stateMachine) { }
}
