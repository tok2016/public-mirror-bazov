using UnityEngine;

/// <summary>
/// Represent the button of Meta controller. Handles colot property of material which represents the button.
/// </summary>
public class MetaButton
{
    private MetaButtonProps _buttonProps;
    private Material _material;
    private MetaStateMachine _stateMachine;

    /// <summary>
    /// Initializes button's properties and state system.
    /// </summary>
    /// <param name="buttonProps">Button's properties for changing the material.</param>
    /// <param name="material">Material of controller model.</param>
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

    /// <summary>
    /// Resets highlight color of button. 
    /// </summary>
    public void ResetColor()
    {
        _material.SetColor(_buttonProps.MaterialField, _buttonProps.DefaultColor);
    }

    /// <summary>
    /// Sets the highlight color of button.
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        _material.SetColor(_buttonProps.MaterialField, color);
    }

    /// <summary>
    /// Sets button state to Warn or previous.
    /// </summary>
    /// <param name="enable">If true, sets button state to Warn.</param>
    public void Warn(bool enable)
    {
        _stateMachine.IsWarned = enable;
    }

    /// <summary>
    /// Sets button state to Pause or previous.
    /// </summary>
    /// <param name="enable">If true, sets button state to Pause.</param>
    public void Pause(bool enable)
    {
        _stateMachine.IsPaused = enable;
    }

    /// <summary>
    /// Sets button state to Pressed or previous.
    /// </summary>
    /// <param name="enable">If true, sets button state to Pressed.</param>
    public void Press(bool enable)
    {
        _stateMachine.IsPressed = enable;
    }

    /// <summary>
    /// Sets button state to Disabled or previous.
    /// </summary>
    /// <param name="enable">If true, sets button state to Disabled.</param>
    public void Disable(bool enable)
    {
        _stateMachine.IsDisabled = true;
    }
}
