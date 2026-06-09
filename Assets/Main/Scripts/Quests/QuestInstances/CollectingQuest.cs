using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CollectingQuest : Quest
{
    [Header("Collecting")]
    [SerializeField] private Bag _bag;

    [Header("Hints")]
    [SerializeField] private HintObject[] _hintObjects;
    [SerializeField] private float _commentaryTimeOffset = 1.5f;
    [SerializeField] private float _timeBetweenHints = 30;
    [SerializeField] private HintZone[] _hintZones;
    private float _hintTimer;
    private Coroutine _commentCoroutine;

    private void OnEnable()
    {
        onFirstEnter.AddListener(EnableHints);
        _bag.OnAllItemsCollected += Check;
    }

    public override void Enter()
    {
        base.Enter();
        _hintTimer = _timeBetweenHints;
    }
    
    protected override void Update()
    {
        //if (_hintTimer <= 0)
        //{
        //    var filtered = _hintZones.Where(zone => zone.Items.Count > 0).ToList();

        //    if (filtered.Count > 0)
        //        filtered[Random.Range(0, filtered.Count)].CommentHint();

        //    _hintTimer = _timeBetweenHints;
        //}
        //else
        //    _hintTimer -= Time.deltaTime;
    }

    protected override void Check()
    {
        Complete();
    }

    internal override void OnItemGrab(SelectEnterEventArgs args)
    {
        var item = args.interactableObject.transform.GetComponent<CollectableItem>();
        if (!item) return;

        Debug.Log(item.Data.Commentary);
        item.WriteWord();

        if (_commentCoroutine != null)
            StopCoroutine(_commentCoroutine);

        _hintTimer = _timeBetweenHints;
    }

    internal override void OnItemLettingGo(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.gameObject.activeInHierarchy 
            && args.interactableObject.transform.GetComponent<CollectableItem>() != null)
            _commentCoroutine = StartCoroutine(CommentLettingGo(args));
    }

    private IEnumerator CommentLettingGo(SelectExitEventArgs args)
    {
        yield return new WaitForSeconds(_commentaryTimeOffset);
        Debug.Log("Положи в суму, так надёжнее будет");
    }

    public override void Complete()
    {
        ToggleHints(false);
        base.Complete();
    }

    public void ToggleHints(bool enable)
    {
        foreach (var zone in _hintZones)
            zone.gameObject.SetActive(enable);

        foreach (var hintObj in _hintObjects)
            hintObj.ToggleOutline(enable);
    }

    private void EnableHints() => ToggleHints(true);

    private void OnDisable()
    {
        onFirstEnter.RemoveListener(EnableHints);
        _bag.OnAllItemsCollected -= Check;
    }
}
