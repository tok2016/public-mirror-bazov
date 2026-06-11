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
        if(State == QuestState.InProgress)
            _controller.CheckItemSpeech();
    }

    protected virtual void OnEnable()
    {
        EnableMainEvents();
        _correctItem.Interactable.selectEntered.AddListener(CheckItem);
    }

    protected void EnableMainEvents()
    {
        onEnterRepeat.AddListener(EnableHints);
    }

    protected void DisableMainEvents()
    {
        onEnterRepeat.RemoveListener(EnableHints);
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
        if (State != QuestState.InProgress) return;

        var item = args.interactableObject.transform.GetComponent<CollectableItem>();
        if (item && item.GetEntityId() == _correctItem.GetEntityId())
        {
            _controller.RemoveItem(item);
            OnCorrectItemGrab.Invoke();
        }
    }

    internal override void OnItemLettingGo(SelectExitEventArgs args)
    {
        if (State != QuestState.InProgress) return;

        var item = args.interactableObject.transform.GetComponent<CollectableItem>();
        if (item)
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

    protected override void Activate()
    {
        EnableHints();
    }

    protected override void Stop()
    {
        DisableHints();
    }

    protected override void Deactivate()
    {
        DisableHints();
    }
}
