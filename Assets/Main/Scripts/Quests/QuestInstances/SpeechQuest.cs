using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SpeechQuest : Quest
{
    [Header("Item Check")]
    [SerializeField] protected CollectableItem _correctItem;
    public UnityEvent OnCorrectItemGrab;
    public UnityEvent OnCorrectItemRelease;

    [Header("Input")]
    [SerializeField] protected SpeechController _controller;
    [SerializeField] protected SpeecControllerhHint _controllerHint;

    protected override void Update()
    {
        _controller.CheckItemSpeech();
    }

    protected virtual void OnEnable()
    {
        EnableMainEvents();
        _correctItem.Interactable.selectEntered.AddListener(CheckItem);
    }

    protected void EnableMainEvents()
    {
        onFirstEnter.AddListener(EnableHints);
        onEnterRepeat.AddListener(EnableHints);
        onEnterAfterComplete.AddListener(EnableHints);
        onComplete.AddListener(DisableHints);
    }

    protected void DisableMainEvents()
    {
        onFirstEnter.RemoveAllListeners();
        onEnterRepeat.RemoveAllListeners();
        onEnterAfterComplete.RemoveAllListeners();
        onComplete.RemoveAllListeners();
    }

    protected void EnableHints() => _controllerHint.ToggleHint(true);
    protected void DisableHints() => _controllerHint.ToggleHint(false);

    private void CheckItem(SelectEnterEventArgs args) => Check();

    protected override void Check()
    {
        Complete();
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

    protected virtual void OnDisable()
    {
        DisableMainEvents();
        _correctItem.Interactable.selectEntered.AddListener(CheckItem);
    }
}
