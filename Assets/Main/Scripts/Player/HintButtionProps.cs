using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains materials for every state of hint button.
/// </summary>
[CreateAssetMenu(fileName = "HintButtionProps", menuName = "Scriptable Objects/HintButtionProps")]
public class HintButtionProps : ScriptableObject
{
    [SerializeField] private Material _defaultMaterial, _warningMaterial, _pauseMaterial, _pressMaterial, _disableMaterial;

    /// <value>
    /// Maps states of hint button with materials.
    /// </value>
    public Dictionary<HintButtonState, Material> StateMaterials { get; private set; }

    private void OnEnable()
    {
        StateMaterials = new Dictionary<HintButtonState, Material>() {
            { HintButtonState.Default, _defaultMaterial },
            { HintButtonState.Disabled, _disableMaterial},
            { HintButtonState.Warn, _warningMaterial},
            { HintButtonState.Pause, _pauseMaterial },
            { HintButtonState.Pressed, _pressMaterial}
        };
    }
}
