using UnityEngine;

public class DictionaryStateMachine
{
    public DictionaryLyingState LyingState {  get; private set; }
    public DictionaryCloseState CloseState { get; private set; }
    public DictionaryGrabState GrabState { get; private set; }
    public DictionaryOpenState OpenState { get; private set; }

    public DictionaryState Current {  get; private set; }

    public DictionaryStateMachine(DictionaryBook book)
    {
        LyingState = new DictionaryLyingState(book, this);
        CloseState = new DictionaryCloseState(book, this);
        GrabState = new DictionaryGrabState(book, this);
        OpenState = new DictionaryOpenState(book, this);

        Current = LyingState;
        Current.Enter();
    }

    public void ChangeState(DictionaryState nextState)
    {
        if (Current.GetType() == nextState.GetType())
            return;

        Current.Exit();
        Current = nextState;
        Current.Enter();
    }

    public void Update()
    {
        Current.Update();
    }
}
