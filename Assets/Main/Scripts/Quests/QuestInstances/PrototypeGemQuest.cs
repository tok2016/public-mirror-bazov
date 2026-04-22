using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class PrototypeGemQuest : ProtoQuest
{
    [SerializeField] private XRGrabInteractable _correctGem;

    internal override void Check(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.gameObject == _correctGem.gameObject)
            Complete();
    }

    internal override void Check(SelectExitEventArgs args)
    {
        throw new System.NotImplementedException();
    }

    internal override void Check(TeleportingEventArgs args)
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Enter();
    }
}
