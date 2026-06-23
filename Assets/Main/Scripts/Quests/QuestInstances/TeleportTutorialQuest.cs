using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportTutorialQuest : TutorialQuest
{
    [SerializeField] private BaseTeleportationInteractable _targetTeleportZone;

    private void OnEnable()
    {
        _targetTeleportZone.teleporting.AddListener(OnTargetTeleport);
    }

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
