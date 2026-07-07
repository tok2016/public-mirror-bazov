/// <summary>
/// Manages highlight/hint state of Meta controller's button.
/// </summary>
public class MetaStateMachine
{
    /// <summary>
    /// Sets state to warned if true.
    /// </summary>
    /// <value>Is state warned.</value>
    public bool IsWarned { get; set; }

    /// <summary>
    /// Sets state to paused if true.
    /// </summary>
    /// <value>Is state paused.</value>
    public bool IsPaused { get; set; }

    /// <summary>
    /// Sets state to pressed if true.
    /// </summary>
    /// <value>Is state pressed.</value>
    public bool IsPressed { get; set; }

    /// <summary>
    /// Sets state to disabled if true.
    /// </summary>
    /// <value>Is state disabled.</value>
    public bool IsDisabled {  get; set; }

    /// <value>
    /// The default state of button highlight.
    /// </value>
    public MetaButtonDefaultState DefaultState { get; private set; }

    /// <value>
    /// The warned state of button highlight.
    /// </value>
    public MetaButtonWarnState WarnState { get; private set; }

    /// <value>
    /// The paused state of button highlight.
    /// </value>
    public MetaButtonPauseState PauseState { get; private set; }

    /// <value>
    /// The pressed state of button highlight.
    /// </value>
    public MetaButtonPressState PressState { get; private set; }

    /// <value>
    /// The disabled state of button highlight.
    /// </value>
    public MetaButtonDisabledState DisabledState { get; private set; }

    private MetaState _current;

    /// <summary>
    /// Initializes state machine properties and states.
    /// </summary>
    /// <param name="metaButton">Button which states the machine will manage.</param>
    /// <param name="props">Button's properties of material.</param>
    public MetaStateMachine(MetaButton metaButton, MetaButtonProps props)
    {
        DefaultState = new MetaButtonDefaultState(metaButton, props.DefaultColor, this);
        WarnState = new MetaButtonWarnState(metaButton, props.WarningColor, this);
        PauseState = new MetaButtonPauseState(metaButton, props.PauseColor, this);
        PressState = new MetaButtonPressState(metaButton, props.PressColor, this);
        DisabledState = new MetaButtonDisabledState(metaButton, props.DisableColor, this);

        _current = DefaultState;
        _current.Enter();
    }

    /// <summary>
    /// Changes current state to given one.
    /// </summary>
    /// <param name="nextState">The next state to replace current one with.</param>
    public void ChangeState(MetaState nextState)
    {
        _current?.Exit();
        _current = nextState;
        _current?.Enter();
    }

    public void Update()
    {
        _current?.Update();
    }
}
