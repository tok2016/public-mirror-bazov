using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[CreateAssetMenu(fileName = "BagQuest", menuName = "Scriptable Objects/Quests/BagQuest")]
public class BagQuest : Quest<int>
{
    public override void Check(SelectEnterEventArgs args)
    {
        if(args.interactableObject.transform.GetComponent<Bag>())
            Complete();
    }

    public override void Complete()
    {
        Debug.Log("Теперь в суму можно складывать предметы.");
        base.Complete();
    }

    public override void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Но сначала нужно найти суму.");
    }
}
