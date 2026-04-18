using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TestGemQuest : Quest
{
    [SerializeField] private XRGrabInteractable _targetGrabItem;

    public override void Check(SelectEnterEventArgs args)
    {
        throw new System.NotImplementedException();
    }

    public override void Complete()
    {
        base.Complete();
    }
}
