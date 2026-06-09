using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SoloninaQuest : SpeechQuest
{
    [Header("Socket")]
    [SerializeField] private XRSocketInteractor _correctItemSocket;

    protected override void OnEnable()
    {
        EnableMainEvents();
        _correctItemSocket.selectEntered.AddListener(CheckItem);
    }

    private void CheckItem(SelectEnterEventArgs args)
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
            Check();
        }
    }

    protected override void OnDisable()
    {
        DisableMainEvents();
        _correctItemSocket.selectEntered.RemoveListener(CheckItem);
    }
}
