using UnityEngine;

/// <summary>
/// Contains material properties that represents button highlight.
/// </summary>
[CreateAssetMenu(fileName = "MetaButtonProps", menuName = "Scriptable Objects/MetaButtonProps")]
public class MetaButtonProps : ScriptableObject
{
    /// <value>
    /// Property of material to be changed to highlight the button.
    /// </value>
    [field: SerializeField] public string MaterialField {  get; private set; }

    /// <value>
    /// Color of material property when highlight is off.
    /// </value>
    [field: SerializeField, ColorUsage(false, true)] public Color DefaultColor { get; private set; }

    /// <value>
    /// Color of material property when button is warning about its availability to be used.
    /// </value>
    [field: SerializeField, ColorUsage(false, true)] public Color WarningColor { get; private set; }

    /// <value>
    /// Color of material property when game is paused.
    /// </value>
    [field: SerializeField, ColorUsage(false, true)] public Color PauseColor { get; private set; }

    /// <value>
    /// Color of material property when button is pressed.
    /// </value>
    [field: SerializeField, ColorUsage(false, true)] public Color PressColor { get; private set; }

    /// <value>
    /// Color of material property when button is disabled.
    /// </value>
    [field: SerializeField, ColorUsage(false, true)] public Color DisableColor { get; private set; }
}
