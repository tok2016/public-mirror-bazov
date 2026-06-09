using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Bag : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private CollectableItem[] _items;
    [SerializeField] private XRSocketInteractor _socket;
    public event Action<CollectableItem> OnItemCollected;
    public event Action OnAllItemsCollected;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _itemBurstEffect;
    [SerializeField] private GameObject _bagBurstPrefab;
    [SerializeField] private float _bagDisapearDelay = 1;

    [Header("Items List UI")]
    [SerializeField] private Transform _itemsGroup;
    [SerializeField] private TextMeshProUGUI _itemLabelPrefab;
    [SerializeField] private Color activeColor, collectedColor;
    private Dictionary<IXRSelectInteractable, CollectableItem> _itemsInteractables;
    private Dictionary<CollectableItem, TextMeshProUGUI> _itemsLabels;

    private void OnEnable()
    {
        _socket.selectEntered.AddListener(CollectItem);
    }

    void Start()
    {
        _itemsInteractables = new Dictionary<IXRSelectInteractable, CollectableItem>();
        _itemsLabels = new Dictionary<CollectableItem, TextMeshProUGUI>();

        foreach (var item in _items)
        {
            _itemsInteractables.Add(item.Interactable, item);

            var itemLabel = Instantiate(_itemLabelPrefab, _itemsGroup);
            itemLabel.text = item.Data.Name;
            itemLabel.color = activeColor;
            _itemsLabels.Add(item, itemLabel);
        }
    }

    public void CollectItem(SelectEnterEventArgs args)
    {
        if (_itemsInteractables.ContainsKey(args.interactableObject))
        {
            var item = _itemsInteractables[args.interactableObject];
            var label = _itemsLabels[item];
            item.TransformSocketedItem();
            label.fontStyle = FontStyles.Strikethrough;
            label.color = collectedColor;

            _itemsInteractables.Remove(args.interactableObject);
            BurstItem();
            OnItemCollected?.Invoke(item);
        }
        else
        {
            Debug.Log("Я думаю, это нам не нужно с собой");
        }

        if (_itemsInteractables.Count <= 0)
            StartCoroutine(CloseBag());
    }

    private void BurstItem()
    {
        if (_itemBurstEffect.gameObject.activeInHierarchy)
        {
            _itemBurstEffect.Clear();
            _itemBurstEffect.Play();
        }
        else
            _itemBurstEffect.gameObject.SetActive(true);
    }

    private IEnumerator<WaitForSeconds> CloseBag()
    {
        Instantiate(_bagBurstPrefab, transform.position, _bagBurstPrefab.transform.rotation, null);
        yield return new WaitForSeconds(_bagDisapearDelay);
        OnAllItemsCollected?.Invoke();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _socket.selectEntered.RemoveAllListeners();
    }
}
