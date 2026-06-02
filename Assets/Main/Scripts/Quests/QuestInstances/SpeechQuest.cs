using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class SpeechQuest : Quest
{
    [Header("Item Check")]
    [SerializeField] private CollectableItem _correctItem;
    public UnityEvent OnCorrectItemGrab;
    public UnityEvent OnCorrectItemRelease;

    [Header("Input")]
    [SerializeField] private SpeechController _controller;
    [SerializeField] private TeleportationProvider _teleportationProvider;
    public UnityEvent OnPadTeleport;

    private void OnEnable()
    {
        _teleportationProvider.locomotionEnded += OnTeleport;
    }

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
        if (State == QuestState.InProgress && item && item.GetEntityId() == _correctItem.GetEntityId())
        {
            _controller.RemoveItem(item);
            OnCorrectItemGrab.Invoke();
        }
    }

    internal override void OnItemLettingGo(SelectExitEventArgs args)
    {
        var item = args.interactableObject.transform.GetComponent<CollectableItem>();
        if (State == QuestState.InProgress && item)
        {
            _controller.AddItem(item);
            if(item.GetEntityId() == _correctItem.GetEntityId())
                OnCorrectItemRelease.Invoke();
        }
    }

    private void OnTeleport(LocomotionProvider provider)
    {
        OnPadTeleport.Invoke();
    }

    private void OnDisable()
    {
        _teleportationProvider.locomotionEnded -= OnTeleport;
    }
}
