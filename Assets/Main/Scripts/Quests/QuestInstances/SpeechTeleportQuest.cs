using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public class SpeechTeleportQuest : SpeechQuest
{
    [SerializeField] private NpcNavigatable[] _charactersToWarp;
    [SerializeField] private TeleportPad _target;

    protected override void OnEnable()
    {
        EnableMainEvents();
        _target.onPadEnter.AddListener(OnTargetTeleport);
    }

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
