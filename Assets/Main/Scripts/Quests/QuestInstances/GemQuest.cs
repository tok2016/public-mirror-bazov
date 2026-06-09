using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GemQuest : Quest
{
    [Header("Check")]
    [SerializeField] private Gem _correctGem;
    [SerializeField] private XRSocketInteractor _correctItemSocket;
    private List<CollectableItem> _gemsInHands;

    protected override void Awake()
    {
        base.Awake();
        _gemsInHands = new List<CollectableItem>();
    }

    private void OnEnable()
    {
        _correctItemSocket.selectEntered.AddListener(CheckGem);
    }

    private void CheckGem(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.GetInstanceID() == _correctGem.transform.GetInstanceID())
            Check();
    }

    protected override void Check()
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

    private void OnDisable()
    {
        _correctItemSocket.selectEntered.RemoveListener(CheckGem);
    }
}
