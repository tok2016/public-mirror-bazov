using UnityEngine;

public abstract class MetaState
{
    protected MetaButton _button;
    protected Color _color;
    protected MetaStateMachine _stateMachine;

    public MetaState(MetaButton metaButton, Color color, MetaStateMachine stateMachine)
    {
        _button = metaButton;
        _color = color;
        _stateMachine = stateMachine;
    } 

    public virtual void Enter()
    {
        _button.SetColor(_color);
    }
    public abstract void Update();
    public virtual void Exit() { }
}
