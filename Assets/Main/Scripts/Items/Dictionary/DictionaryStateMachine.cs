using UnityEngine;

public class DictionaryStateMachine
{
    public DictionaryLyingState LyingState {  get; private set; }
    public DictionaryCloseState CloseState { get; private set; }
    public DictionaryOpenState OpenState { get; private set; }

    private DictionaryState _current;

    public DictionaryStateMachine(DictionaryBook book)
    {
        LyingState = new DictionaryLyingState(book);
        CloseState = new DictionaryCloseState(book);
        OpenState = new DictionaryOpenState(book);

        _current = LyingState;
        _current.Enter(null);
    }

    public void ChangeState(DictionaryState nextState, Transform parent)
    {
        if (_current.GetType() == nextState.GetType())
            return;

        _current.Exit();
        _current = nextState;
        _current.Enter(parent);
    }

    public void Update()
    {
        _current.Update();
    }
}
