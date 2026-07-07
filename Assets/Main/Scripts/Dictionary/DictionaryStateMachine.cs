using UnityEngine;

/// <summary>
/// Manages physical behaviour states of dictionary as a book.
/// </summary>
public class DictionaryStateMachine
{
    /// <value>
    /// State of book behaviour when it's lying and isn't owned by player.
    /// </value>
    public DictionaryLyingState LyingState {  get; private set; }

    /// <value>
    /// State of book behaviour when it's closed.
    /// </value>
    public DictionaryCloseState CloseState { get; private set; }

    /// <value>
    /// State of book behaviour when it's grabbed by player.
    /// </value>
    public DictionaryGrabState GrabState { get; private set; }

    /// <value>
    /// State of book behaviour when it's opened.
    /// </value>
    public DictionaryOpenState OpenState { get; private set; }

    /// <summary>
    /// Current state of book behaviour.
    /// </summary>
    public DictionaryState Current {  get; private set; }

    /// <summary>
    /// Initializes state machine properties and states.
    /// </summary>
    /// <param name="book">Book to controll.</param>
    public DictionaryStateMachine(DictionaryBook book)
    {
        LyingState = new DictionaryLyingState(book, this);
        CloseState = new DictionaryCloseState(book, this);
        GrabState = new DictionaryGrabState(book, this);
        OpenState = new DictionaryOpenState(book, this);

        Current = LyingState;
        Current.Enter();
    }

    /// <summary>
    /// Replaces currently active book state with the given one. Does nothing when state are the same.
    /// </summary>
    /// <param name="nextState">State to replace the current one with.</param>
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
