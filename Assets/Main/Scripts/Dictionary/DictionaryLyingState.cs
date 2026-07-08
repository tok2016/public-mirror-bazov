using UnityEngine;

/// <summary>
/// Controlls the book when it's lying and isn't owned by the player. Does nothing.
/// </summary>
public class DictionaryLyingState : DictionaryState
{
    public DictionaryLyingState(DictionaryBook book, DictionaryStateMachine stateMachine) : base(book, stateMachine) { }
}
