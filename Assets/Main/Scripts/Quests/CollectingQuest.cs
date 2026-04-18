using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CollectingQuest : Quest
{
    [SerializeField] private CollectableItem[] _items;
    [SerializeField] private HintZone[] _hintZones;
    private Dictionary<IXRSelectInteractable, CollectableItem> _objectsCount;

    [SerializeField] private float _timeBetweenHints = 15;
    private float _hintTimer;

    private void Awake()
    {
        _objectsCount = new Dictionary<IXRSelectInteractable, CollectableItem>();
        foreach (var item in _items)
        {
            var intractable = item.GetComponent<XRGrabInteractable>();
            if(intractable)
                _objectsCount.Add(intractable, item);
        }
    }

    protected override void Start()
    {
        base.Start();
        _hintTimer = _timeBetweenHints;
    }

    protected override void Update()
    {
        if(_hintTimer <= 0)
        {
            var filtered = _hintZones.Where(zone => zone.Items.Count > 0).ToList();

            if(filtered.Count > 0)
            {
                filtered[Random.Range(0, filtered.Count)].CommentHint();
                _hintTimer = _timeBetweenHints;
            }
        }
        else
            _hintTimer -= Time.deltaTime;
    }

    public void onItemGrab(SelectEnterEventArgs args)
    {
        _hintTimer = _timeBetweenHints;
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
        foreach(var item in _items)
            item.gameObject.SetActive(false);
        foreach(var zone in _hintZones)
            Destroy(zone.gameObject);
        base.Complete();
    }
}
