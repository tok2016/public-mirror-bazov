using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BagQuest : Quest
{
    protected override void Start()
    {
        base.Start();
        Enter();
    }

    public override void Check(SelectEnterEventArgs args)
    {
        Complete();
    }

    public override void Complete()
    {
        Debug.Log("Теперь в суму можно складывать предметы.");
        base.Complete();
    }

    public void CommentItemGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Но сначала надо найти суму.");
    }
}
