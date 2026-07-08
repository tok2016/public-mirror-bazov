using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

/// <summary>
/// Checks for item call quest connected to teleportation to be completed.
/// </summary>
public class SpeechTeleportQuest : SpeechQuest
{
    [SerializeField] private NpcNavigatable[] _charactersToWarp;
    [SerializeField] private TeleportPad _target;

    protected override void OnEnable()
    {
        EnableMainEvents();
        _target.onPadEnter.AddListener(OnTargetTeleport);
    }

    /// <summary>
    /// Checks for entered pad to be correct.
    /// </summary>
    /// <param name="teleportPad">Pad player has teleported to.</param>
    private void OnTargetTeleport(TeleportPad teleportPad) => Check();

    internal override void OnTeleportEnd(LocomotionProvider provider)
    {
        foreach (var character in _charactersToWarp)
            character.WarpToPlayer();
    }

    protected override void OnDisable()
    {
        DisableMainEvents();
        _target.onPadEnter.RemoveListener(OnTargetTeleport);
    }  
}
