using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CollectingQuest : Quest
{
    [Header("Collecting")]
    [SerializeField] private Bag _bag;

    [Header("Hints")]
    [SerializeField] private HintObject[] _hintObjects;
    [SerializeField] private HintZone[] _hintZones;
    private Coroutine _commentCoroutine;

    protected override void Awake()
    {
        base.Awake();
        ToggleHints(false);
    }

    private void OnEnable()
    {
        _bag.OnAllItemsCollected += Check;
    }

    protected override void Check()
    {
        Complete();
    }

    internal override void OnItemGrab(SelectEnterEventArgs args)
    {
        if (State != QuestState.InProgress) return;

        var item = args.interactableObject.transform.GetComponent<CollectableItem>();
        if (!item) return;

        item.CommentGrab();
        if (_commentCoroutine != null)
            StopCoroutine(_commentCoroutine);
    }

    public void ToggleHints(bool enable)
    {
        _bag.ToggleBag(enable);

        foreach (var zone in _hintZones)
            zone.gameObject.SetActive(enable);

        foreach (var hintObj in _hintObjects)
            hintObj.ToggleOutline(enable);
    }

    private void OnDisable()
    {
        _bag.OnAllItemsCollected -= Check;
    }

    protected override void Activate()
    {
        ToggleHints(true);
    }

    protected override void Stop()
    {
        
    }

    protected override void Deactivate()
    {
        ToggleHints(false);
    }
}
