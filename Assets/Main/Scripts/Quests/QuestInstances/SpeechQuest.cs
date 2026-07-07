using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Checks for item call quest completion. 
/// </summary>
public class SpeechQuest : Quest
{
    [Header("Item Check")]
    [SerializeField] protected GrabbableObject _correctItem;
    [SerializeField] protected Bag _bag;
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

    /// <summary>
    /// Adds handlers to all necessary events.
    /// </summary>
    protected void EnableMainEvents()
    {
        onReturn.AddListener(EnableHints);
    }

    /// <summary>
    /// Removes handlers from all necessary events.
    /// </summary>
    protected void DisableMainEvents()
    {
        onReturn.RemoveListener(EnableHints);
    }

    /// <summary>
    /// Enables speech controller hints.
    /// </summary>
    protected void EnableHints()
    {
        _controllerHint.ToggleHint(true);
    }

    /// <summary>
    /// Disables speech controller hints.
    /// </summary>
    protected void DisableHints()
    {
        _controllerHint.ToggleHint(false);
    }

    /// <summary>
    /// Compares grabbed item with correct one.
    /// </summary>
    private void CheckItem(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.GetEntityId() == _correctItem.GetEntityId())
            Check();
    }

    protected override void Check()
    {
        Complete();
    }

    internal override void OnItemGrab(SelectEnterEventArgs args)
    {
        if (State != QuestState.InProgress) return;

        var item = args.interactableObject.transform.GetComponent<GrabbableObject>();
        if (item && item.GetEntityId() == _correctItem.GetEntityId())
        {
            _bag.RemoveItem(item);
            OnCorrectItemGrab.Invoke();
        }
    }

    internal override void OnItemLettingGo(SelectExitEventArgs args)
    {
        var item = args.interactableObject.transform.GetComponent<GrabbableObject>();
        if (item)
        {
            _bag.AddItem(item);
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
