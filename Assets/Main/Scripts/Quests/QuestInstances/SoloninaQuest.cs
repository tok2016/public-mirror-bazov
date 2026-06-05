using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

public class SoloninaQuest : SpeechQuest
{
    internal override void Check(SelectEnterEventArgs args)
    {
        var item = args.interactableObject.transform.GetComponent<CollectableItem>();
        if (item && item.GetEntityId() == _correctItem.GetEntityId())
        {
            var npcGrabbable = item.GetComponent<NpcGrabbable>();
            if (!npcGrabbable)
                npcGrabbable = item.AddComponent<NpcGrabbable>();

            npcGrabbable.enabled = true;
            item.Interactable.interactionLayers = InteractionLayerMask.NameToLayer("Nothing");

            _controller.RemoveItem(item);
            Complete();
        }
    }
}
