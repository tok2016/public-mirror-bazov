using UnityEngine;

/// <summary>
/// Base class for all states of Meta controller's button highlights.
/// </summary>
public abstract class MetaState
{
    protected MetaButton _button;
    protected Color _color;
    protected MetaStateMachine _stateMachine;

    /// <summary>
    /// Initialized state properties.
    /// </summary>
    /// <param name="metaButton">Button which the state belongs to.</param>
    /// <param name="color">Highlight color of state.</param>
    /// <param name="stateMachine">Machine to change the state.</param>
    public MetaState(MetaButton metaButton, Color color, MetaStateMachine stateMachine)
    {
        _button = metaButton;
        _color = color;
        _stateMachine = stateMachine;
    } 

    /// <summary>
    /// Runs logic when entering the state.
    /// </summary>
    public virtual void Enter()
    {
        _button.SetColor(_color);
    }

    /// <summary>
    /// Runs logic that checks state changing.
    /// </summary>
    public abstract void Update();

    /// <summary>
    /// Runs logic when exiting the state.
    /// </summary>
    public virtual void Exit() { }
}
