using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SpeechQuest : Quest
{
    [SerializeField] private CollectableItem _correctItem;
    [SerializeField] private SpeechController _controller;
    public UnityEvent OnCorrectItemGrab;
    public UnityEvent OnCorrectItemRelease;

    protected override void Update()
    {
        _controller.CheckItemSpeech();
    }

    internal override void Check(SelectEnterEventArgs args)
    {
        var item = args.interactableObject.transform.GetComponent<CollectableItem>();
        if (item && item.GetEntityId() == _correctItem.GetEntityId())
        {
            _controller.RemoveItem(item);
            item.gameObject.SetActive(false);
            Complete();
        }    
    }

    internal override void OnItemGrab(SelectEnterEventArgs args)
    {
        var item = args.interactableObject.transform.GetComponent<CollectableItem>();
        if (item && item.GetEntityId() == _correctItem.GetEntityId())
        {
            _controller.RemoveItem(item);
            OnCorrectItemGrab.Invoke();
        }
    }

    internal override void OnItemLettingGo(SelectExitEventArgs args)
    {
        var item = args.interactableObject.transform.GetComponent<CollectableItem>();
        if (item)
        {
            _controller.AddItem(item);
            if(item.GetEntityId() == _correctItem.GetEntityId())
                OnCorrectItemRelease.Invoke();
        }
    }
}
