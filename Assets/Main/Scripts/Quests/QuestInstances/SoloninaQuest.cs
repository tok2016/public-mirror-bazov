using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Checks for item call quest connected to socket to be completed.
/// </summary>
public class SoloninaQuest : SpeechQuest
{
    [Header("Socket")]
    [SerializeField] private XRSocketInteractor _correctItemSocket;
    [SerializeField] private GameObject _successEffect;

    protected override void OnEnable()
    {
        EnableMainEvents();
        _correctItemSocket.selectEntered.AddListener(CheckItem);
    }

    /// <summary>
    /// Compares socketed item with the correct one and transforms it to be interactable with NPC.
    /// </summary>
    /// <param name="args"></param>
    private void CheckItem(SelectEnterEventArgs args)
    {
        var item = args.interactableObject.transform.GetComponent<GrabbableObject>();
        if (item && item.GetEntityId() == _correctItem.GetEntityId())
        {
            var npcGrabbable = item.GetComponent<NpcGrabbable>();
            if (!npcGrabbable)
                npcGrabbable = item.AddComponent<NpcGrabbable>();

            npcGrabbable.enabled = true;
            item.ToggleInteractivity(false);
            item.transform.position = _correctItemSocket.attachTransform.position;
            _successEffect.SetActive(true);

            _bag.RemoveItem(item);
            Check();
        }
    }

    protected override void OnDisable()
    {
        DisableMainEvents();
        _correctItemSocket.selectEntered.RemoveListener(CheckItem);
    }

    protected override void Activate()
    {
        base.Activate();
        _correctItemSocket.gameObject.SetActive(true);
    }

    protected override void Deactivate()
    {
        base.Deactivate();
        _correctItemSocket.gameObject.SetActive(false);
    }
}
