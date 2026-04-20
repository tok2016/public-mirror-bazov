using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[System.Serializable]
public struct CollectingQuestProps
{
    public CollectableItem[] items;
    public HintZone[] hintZones;
}

[CreateAssetMenu(fileName = "CollectingQuest", menuName = "Scriptable Objects/Quests/CollectingQuest")]
public class CollectingQuest : Quest<CollectingQuestProps>
{
    private CollectableItem[] _items;
    private HintZone[] _hintZones;
    private Dictionary<IXRSelectInteractable, CollectableItem> _objectsCount;

    [SerializeField] private float _timeBetweenHints = 30;
    private float _hintTimer;

    public override void Enter(CollectingQuestProps props)
    {
        base.Enter(props);

        _items = props.items;
        _hintZones = props.hintZones;

        _objectsCount = new Dictionary<IXRSelectInteractable, CollectableItem>();
        _hintTimer = _timeBetweenHints;

        foreach (var item in _items)
        {
            var intractable = item.GetComponent<XRGrabInteractable>();
            if (intractable)
                _objectsCount.Add(intractable, item);
        }
    }

    public override void Update()
    {
        if (_hintTimer <= 0)
        {
            var filtered = _hintZones.Where(zone => zone.Items.Count > 0).ToList();

            if (filtered.Count > 0)
            {
                filtered[Random.Range(0, filtered.Count)].CommentHint();
                _hintTimer = _timeBetweenHints;
            }
        }
        else
            _hintTimer -= Time.deltaTime;
    }

    public override void OnGrab(SelectEnterEventArgs args)
    {
        _hintTimer = _timeBetweenHints;
    }

    public override void OnLettingGo(SelectExitEventArgs args)
    {
        Debug.Log("Положи в суму, так надёжнее будет");
    }

    public override void Check(SelectEnterEventArgs args)
    {
        if (_objectsCount.ContainsKey(args.interactableObject))
        {
            var a = _objectsCount[args.interactableObject];
            a.TransformSocketedItem();
            _objectsCount.Remove(args.interactableObject);
        }


        if (_objectsCount.Count == 0)
            Complete();
    }

    public override void Complete()
    {
        foreach (var item in _items)
            item.gameObject.SetActive(false);
        foreach (var zone in _hintZones)
            zone.gameObject.SetActive(false);
        base.Complete();
    }
}
