using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class SpeechTeleportQuest : SpeechQuest
{
    [SerializeField] private TeleportPad _target;
    public UnityEvent OnPadTeleport;

    internal override void OnTeleport(TeleportingEventArgs args)
    {
        if(args.interactableObject.transform.GetEntityId() == _target.Anchor.transform.GetEntityId())
            Complete();
    }

    internal override void OnTeleportEnd(LocomotionProvider provider)
    {
        OnPadTeleport.Invoke();
    }
}
