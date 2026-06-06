using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GemQuest : Quest
{
    private List<CollectableItem> _gemsInHands;

    protected override void Awake()
    {
        base.Awake();
        _gemsInHands = new List<CollectableItem>();
    }

    internal override void Check(SelectEnterEventArgs args)
    {
        Complete();
    }

    internal override void OnItemGrab(SelectEnterEventArgs args)
    {
        var gem = args.interactableObject.transform.GetComponent<CollectableItem>();
        if (gem)
        {
            _gemsInHands.Add(gem);
            gem.WriteWord();
        }
            
        if (_gemsInHands.Count > 1)
            Debug.Log("Один камешек - хорошо, а два - уж лишнее будет. Выбери самый заветный");
    }

    internal override void OnItemLettingGo(SelectExitEventArgs args)
    {
        var gem = args.interactableObject.transform.GetComponent<CollectableItem>();
        if (gem && _gemsInHands.Contains(gem))
            _gemsInHands.Remove(gem);
    }
}
