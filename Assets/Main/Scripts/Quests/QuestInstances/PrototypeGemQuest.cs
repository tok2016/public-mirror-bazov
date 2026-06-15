using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class PrototypeGemQuest : ProtoQuest
{
    [SerializeField] private XRGrabInteractable _correctGem;

    protected override IXRSelectInteractable[] ImportantItems => throw new System.NotImplementedException();

    public void Check(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.gameObject == _correctGem.gameObject)
            Complete();
    }

    protected override void Check()
    {
        Complete();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Enter();
    }
}
