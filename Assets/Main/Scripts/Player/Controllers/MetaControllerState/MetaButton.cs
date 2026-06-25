using UnityEngine;

public class MetaButton
{
    private MetaButtonProps _buttonProps;
    private Material _material;
    private MetaStateMachine _stateMachine;

    public MetaButton(MetaButtonProps buttonProps, Material material)
    {
        _buttonProps = buttonProps;
        _material = material;
        _stateMachine = new MetaStateMachine(this, buttonProps);
    }

    public void Update()
    {
        _stateMachine.Update();
    }

    public void ResetColor()
    {
        _material.SetColor(_buttonProps.MaterialField, Color.black);
    }

    public void SetColor(Color color)
    {
        _material.SetColor(_buttonProps.MaterialField, color);
    }

    public void Warn(bool enable)
    {
        _stateMachine.IsWarned = enable;
    }

    public void Pause(bool enable)
    {
        _stateMachine.IsPaused = enable;
    }

    public void Press(bool enable)
    {
        _stateMachine.IsPressed = enable;
    }

    public void Disable(bool enable)
    {
        _stateMachine.IsDisabled = true;
    }
}
