using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

/// <summary>
/// Checks for teleportation tutorial quest completion.
/// </summary>
public class TeleportTutorialQuest : TutorialQuest
{
    [SerializeField] private BaseTeleportationInteractable _targetTeleportZone;

    private void OnEnable()
    {
        _targetTeleportZone.teleporting.AddListener(OnTargetTeleport);
    }

    /// <summary>
    /// Cheks if the player teleported to target zone.
    /// </summary>
    /// <param name="args"></param>
    private void OnTargetTeleport(TeleportingEventArgs args)
    {
        Check();
    }

    private void OnDisable()
    {
        _targetTeleportZone.teleporting.RemoveListener(OnTargetTeleport);
    }

    protected override void ToggleControllersHint(bool enable)
    {
        _rightController.WarnAboutTeleport(enable);
    }
}
