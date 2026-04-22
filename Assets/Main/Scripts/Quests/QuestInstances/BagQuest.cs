using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class BagQuest : Quest
{
    internal override void Check(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.GetComponent<Bag>())
            Complete();
    }

    internal override void OnItemGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Но сначала нужно найти суму.");
    }
}
