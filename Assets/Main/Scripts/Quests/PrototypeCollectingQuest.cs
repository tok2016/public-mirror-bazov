using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[System.Serializable]
struct ItemLabel
{
    public string name;
    public XRGrabInteractable item;
    public TextMeshProUGUI label;
}

public class PrototypeCollectingQuest : Quest<QuestData>
{
    [SerializeField] private ItemLabel[] _objectsToCollect;
    private Dictionary<IXRSelectInteractable, ItemLabel> _itemsLabels;
    private Dictionary<IXRSelectInteractable, int> _objectsCount;

    private void Awake()
    {
        _itemsLabels = new Dictionary<IXRSelectInteractable, ItemLabel>();
        _objectsCount = new Dictionary<IXRSelectInteractable, int>();
        foreach (var obj in _objectsToCollect)
        {
            _itemsLabels.Add(obj.item, obj);
            _objectsCount.Add(obj.item, 1);
        }
            
    }

    protected override void Start()
    {
        base.Start();
        Enter();
        foreach (var itemLabel in _itemsLabels.Values)
            itemLabel.label.text = itemLabel.name;
    }

    public void Check(SelectEnterEventArgs args)
    {
        if(_itemsLabels.ContainsKey(args.interactableObject) && _objectsCount.ContainsKey(args.interactableObject)) 
        {
            _itemsLabels[args.interactableObject].label.fontStyle = FontStyles.Strikethrough;
            _itemsLabels[args.interactableObject].label.color = Color.gray7;
            _objectsCount.Remove(args.interactableObject);
            Destroy(args.interactableObject.transform.gameObject);
        }
            

        if (_objectsCount.Count == 0)
            Complete();
    }

    public override void Complete()
    {
        base.Complete();
        foreach (var itemLabel in _itemsLabels.Values)
            Destroy(itemLabel.label.gameObject);
    }
}
