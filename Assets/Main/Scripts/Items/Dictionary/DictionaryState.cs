using UnityEngine;

public abstract class DictionaryState
{
    protected DictionaryBook _book;
    protected DictionaryStateMachine _stateMachine;

    public DictionaryState(DictionaryBook book, DictionaryStateMachine stateMachine)
    {
        _book = book;
        _stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
