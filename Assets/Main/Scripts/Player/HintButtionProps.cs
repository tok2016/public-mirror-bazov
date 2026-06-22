using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HintButtionProps", menuName = "Scriptable Objects/HintButtionProps")]
public class HintButtionProps : ScriptableObject
{
    [SerializeField] private Material _defaultMaterial, _warningMaterial, _pauseMaterial, _pressMaterial, _disableMaterial;

    public Dictionary<HintButtonState, Material> StateMaterials { get; private set; }

    private void OnEnable()
    {
        StateMaterials = new Dictionary<HintButtonState, Material>() {
            { HintButtonState.Default, _defaultMaterial },
            { HintButtonState.Disabled, _disableMaterial},
            { HintButtonState.Active, _warningMaterial},
            { HintButtonState.UI, _pauseMaterial },
            { HintButtonState.Pressed, _pressMaterial}
        };
    }
}
