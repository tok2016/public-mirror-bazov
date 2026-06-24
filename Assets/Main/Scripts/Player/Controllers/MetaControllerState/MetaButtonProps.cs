using UnityEngine;

[CreateAssetMenu(fileName = "MetaButtonProps", menuName = "Scriptable Objects/MetaButtonProps")]
public class MetaButtonProps : ScriptableObject
{
    [field: SerializeField] public string MaterialField {  get; private set; }
    [field: SerializeField, ColorUsage(false, true)] public Color DefaultColor { get; private set; }
    [field: SerializeField, ColorUsage(false, true)] public Color WarningColor { get; private set; }
    [field: SerializeField, ColorUsage(false, true)] public Color PauseColor { get; private set; }
    [field: SerializeField, ColorUsage(false, true)] public Color PressColor { get; private set; }
    [field: SerializeField, ColorUsage(false, true)] public Color DisableColor { get; private set; }
}
