using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PrototypeGemQuest : ProtoQuest
{
    [SerializeField] private XRGrabInteractable _correctGem;
    
    public override void Check(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.gameObject == _correctGem.gameObject)
        {
            Complete();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Enter();
    }
}
