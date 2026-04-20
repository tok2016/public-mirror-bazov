using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[System.Serializable]
public struct ItemLabel
{
    public string name;
    public XRGrabInteractable item;
    public TextMeshProUGUI label;
}

[System.Serializable]
public struct ProtoCollectingProps
{
    public TextMeshProUGUI title;
    public ItemLabel[] objectsToCollect;
}

[CreateAssetMenu(fileName = "ProtoCollectingQuest", menuName = "Scriptable Objects/Quests/ProtoCollectingQuest")]
public class ProtoCollectingQuest : Quest<ProtoCollectingProps>
{
    private TextMeshProUGUI _title;
    private ItemLabel[] _objectsToCollect;
    private Dictionary<IXRSelectInteractable, ItemLabel> _itemsLabels;
    private Dictionary<IXRSelectInteractable, int> _objectsCount;

    public override void Enter(ProtoCollectingProps props)
    {
        base.Enter(props);

        _title = props.title;
        _objectsToCollect = props.objectsToCollect;

        _itemsLabels = new Dictionary<IXRSelectInteractable, ItemLabel>();
        _objectsCount = new Dictionary<IXRSelectInteractable, int>();

        foreach (var obj in _objectsToCollect)
        {
            _itemsLabels.Add(obj.item, obj);
            _objectsCount.Add(obj.item, 1);
        }

        foreach (var itemLabel in _itemsLabels.Values)
            itemLabel.label.text = itemLabel.name;

        _title.text = Name;
        _title.fontStyle = FontStyles.Normal;
    }

    public override void Check(SelectEnterEventArgs args)
    {
        if (_itemsLabels.ContainsKey(args.interactableObject) && _objectsCount.ContainsKey(args.interactableObject))
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
        _title.fontStyle = FontStyles.Strikethrough;
        foreach (var itemLabel in _itemsLabels.Values)
            Destroy(itemLabel.label.gameObject);
    }
}
