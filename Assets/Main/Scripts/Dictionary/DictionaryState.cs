using UnityEngine;

/// <summary>
/// Base class for all dictionary book states that controll its physical behaviour.
/// </summary>
public abstract class DictionaryState
{
    protected DictionaryBook _book;
    protected DictionaryStateMachine _stateMachine;

    /// <summary>
    /// Initializes state properties common for every of them.
    /// </summary>
    /// <param name="book">Dictionary book to controll.</param>
    /// <param name="stateMachine">State machine to change the state.</param>
    public DictionaryState(DictionaryBook book, DictionaryStateMachine stateMachine)
    {
        _book = book;
        _stateMachine = stateMachine;
    }

    /// <summary>
    /// Runs logic when enterting the state.
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// Runs logic every frame.
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// Runs logic when exiting the state.
    /// </summary>
    public virtual void Exit() { }
}
