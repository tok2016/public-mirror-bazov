using UnityEngine;

public abstract class DictionaryState
{
    protected DictionaryBook _book;

    public DictionaryState(DictionaryBook book)
    {
        _book = book;
    }

    public virtual void Enter(Transform parent) 
    {
        //_book.transform.SetParent(parent);
    }
    public virtual void Update() { }
    public virtual void Exit() { }
}
