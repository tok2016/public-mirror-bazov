using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TestGemQuest : Quest<QuestData>
{
    [SerializeField] private XRGrabInteractable _targetGrabItem;


    public override void Complete()
    {
        base.Complete();
    }
}
