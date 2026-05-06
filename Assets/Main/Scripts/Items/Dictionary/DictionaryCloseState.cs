using UnityEngine;

public class DictionaryCloseState : DictionaryState
{
    public DictionaryCloseState(DictionaryBook book) : base(book) { }

    public override void Enter(Transform parent)
    {
        base.Enter(parent);
        _book.transform.localRotation = Quaternion.identity;
        _book.transform.rotation = Quaternion.identity;
        _book.transform.localPosition = Vector3.zero;
    }
}
