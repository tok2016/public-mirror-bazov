using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CollectingQuest : Quest
{
    [Header("Items")]
    [SerializeField] private CollectableItem[] _items;
    [SerializeField] private Transform _bag;
    private Dictionary<IXRSelectInteractable, CollectableItem> _objectsCount;

    [Header("Hints")]
    [SerializeField] private float _commentaryTimeOffset = 1.5f;
    [SerializeField] private float _timeBetweenHints = 30;
    [SerializeField] private HintZone[] _hintZones;
    private float _hintTimer;
    private Coroutine _commentCoroutine;

    public UnityEvent<Transform> onItemsCollected;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _textCounter;
    [SerializeField] private TextMeshProUGUI _textCount;

    public override void Enter()
    {
        base.Enter();

        _objectsCount = new Dictionary<IXRSelectInteractable, CollectableItem>();
        _hintTimer = _timeBetweenHints;

        foreach (var item in _items)
        {
            var intractable = item.GetComponent<XRGrabInteractable>();
            if (intractable)
                _objectsCount.Add(intractable, item);
        }

        _textCount.text = _items.Length.ToString();
        _textCounter.text = "0";
    }

    protected override void Update()
    {
        if (_hintTimer <= 0)
        {
            var filtered = _hintZones.Where(zone => zone.Items.Count > 0).ToList();

            if (filtered.Count > 0)
                filtered[Random.Range(0, filtered.Count)].CommentHint();

            _hintTimer = _timeBetweenHints;
        }
        else
            _hintTimer -= Time.deltaTime;
    }

    internal override void OnItemGrab(SelectEnterEventArgs args)
    {
        var item = args.interactableObject.transform.GetComponent<CollectableItem>();
        if (args.interactorObject.GetType() != typeof(XRSocketInteractor))
            Debug.Log(item.Data.Commentary);

        item.WriteWord();

        if (_commentCoroutine != null)
            StopCoroutine(_commentCoroutine);

        _hintTimer = _timeBetweenHints;
    }

    internal override void OnItemLettingGo(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.gameObject.activeInHierarchy)
            _commentCoroutine = StartCoroutine(CommentLettingGo(args));
    }

    private IEnumerator CommentLettingGo(SelectExitEventArgs args)
    {
        yield return new WaitForSeconds(_commentaryTimeOffset);
        Debug.Log("Положи в суму, так надёжнее будет");
    }

    internal override void Check(SelectEnterEventArgs args)
    {
        if (_objectsCount.ContainsKey(args.interactableObject))
        {
            var a = _objectsCount[args.interactableObject];
            a.TransformSocketedItem();
            _objectsCount.Remove(args.interactableObject);

            _textCounter.text = (_items.Length - _objectsCount.Count).ToString();
        }
        else
        {
            Debug.Log("Я думаю, это нам не нужно с собой");
        }

        if (_objectsCount.Count == 0)
            Complete();
    }

    public override void Complete()
    {
        foreach (var zone in _hintZones)
            zone.gameObject.SetActive(false);

        onItemsCollected.Invoke(_bag);

        base.Complete();
    }
}
