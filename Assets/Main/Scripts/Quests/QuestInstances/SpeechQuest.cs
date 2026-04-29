using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpeechQuest : Quest
{
    [SerializeField] private CollectableItem _correctItem;
    [SerializeField] private WordData _word;

    internal override void Check(SelectEnterEventArgs args)
    {
        var item = args.interactableObject.transform.GetComponent<CollectableItem>();
        if (item && item.Data.Word.Title == _word.Title)
            Complete();
    }

    internal override void OnItemGrab(SelectEnterEventArgs args)
    {
        Check(args);
    }
}
