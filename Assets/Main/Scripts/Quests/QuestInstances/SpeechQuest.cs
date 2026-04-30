using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpeechQuest : Quest
{
    [SerializeField] private CollectableItem _correctItem;
    [SerializeField] private WordData _word;
    [SerializeField] private SpeechController _controller;

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
}
