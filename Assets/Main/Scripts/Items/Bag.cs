using FuzzySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Manages collectable items.
/// </summary>
public class Bag : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GrabbableObject[] _items;
    [SerializeField] private GameObject _bagModel;
    [SerializeField] private XRSocketInteractor _socket;
    private List<GrabbableObject> _collectedItems;
    public event Action<GrabbableObject> OnItemCollected;
    public event Action OnAllItemsCollected;

    [Header("Effects")]
    [SerializeField] private GameObject _itemBurstEffect;
    [SerializeField] private GameObject _bagBurstPrefab;
    [SerializeField] private float _bagDisapearDelay = 1;

    [Header("Items List UI")]
    [SerializeField] private Transform _itemsGroup;
    [SerializeField] private TextMeshProUGUI _itemLabelPrefab;
    [SerializeField] private Color activeColor, collectedColor;
    private Dictionary<IXRSelectInteractable, GrabbableObject> _itemsInteractables;
    private Dictionary<GrabbableObject, TextMeshProUGUI> _itemsLabels;

    [Header("Items Call")]
    [SerializeField] private SpeechController _speechController;
    [SerializeField] private float _itemScoreThreshold = 33;
    [SerializeField] private Transform _itemAttachPoint;
    public event Action<IGrabbable> OnItemFound;

    private void OnEnable()
    {
        _socket.selectEntered.AddListener(CollectItem);
        _speechController.onTranscribed += InstantiateItemByTitle;
    }

    void Start()
    {
        _collectedItems = new List<GrabbableObject>();
        _itemsInteractables = new Dictionary<IXRSelectInteractable, GrabbableObject>();
        _itemsLabels = new Dictionary<GrabbableObject, TextMeshProUGUI>();

        foreach (var item in _items)
        {
            _itemsInteractables.Add(item.Interactable, item);

            var itemLabel = Instantiate(_itemLabelPrefab, _itemsGroup);
            itemLabel.text = item.Data.Name;
            itemLabel.color = activeColor;
            _itemsLabels.Add(item, itemLabel);
        }
    }

    /// <summary>
    /// Makes item inactive and adds it to collected list.
    /// </summary>
    /// <param name="args"></param>
    public void CollectItem(SelectEnterEventArgs args)
    {
        if (_itemsInteractables.ContainsKey(args.interactableObject))
        {
            var item = _itemsInteractables[args.interactableObject];
            var label = _itemsLabels[item];
            item.TransformSocketedItem();
            label.fontStyle = FontStyles.Strikethrough;
            label.color = collectedColor;

            _collectedItems.Add(item);
            _itemsInteractables.Remove(args.interactableObject);
            BurstItem();
            OnItemCollected?.Invoke(item);
        }
        else
            Debug.Log("Я думаю, это нам не нужно с собой");

        if (_itemsInteractables.Count <= 0)
            StartCoroutine(CloseBag());
    }

    /// <summary>
    /// Starts item collection effect.
    /// </summary>
    private void BurstItem()
    {
        if (_itemBurstEffect.activeInHierarchy)
            _itemBurstEffect.SetActive(false);
        _itemBurstEffect.gameObject.SetActive(true);
    }

    /// <summary>
    /// Waits for bag disappearance.
    /// </summary>
    /// <returns></returns>
    private IEnumerator<WaitForSeconds> CloseBag()
    {
        Instantiate(_bagBurstPrefab, _bagModel.transform.position, _bagBurstPrefab.transform.rotation, null);
        yield return new WaitForSeconds(_bagDisapearDelay);
        OnAllItemsCollected?.Invoke();
        _bagModel.SetActive(false);
    }

    /// <summary>
    /// Enables or disables bag game object.
    /// </summary>
    /// <param name="enable">Whether to enable or disable bag.</param>
    public void ToggleBag(bool enable)
    {
        _socket.enabled = enable;
        _itemsGroup.gameObject.SetActive(enable);
    }

    /// <summary>
    /// Finds item by <c>WordData</c> title and make it active and interactable. 
    /// </summary>
    /// <param name="text">Text with item title</param>
    /// <exception cref="Exception">Thrown when no item has word or now item was found by word title</exception>
    public void InstantiateItemByTitle(string text)
    {
        var words = _collectedItems
            .Where(item => item.Data.DialogueLine && item.Data.DialogueLine.Word)
            .Select(item => item.Data.DialogueLine.Word.Title.ToLower().Replace('ё', 'е'))
            .ToArray();

        if (words.Length == 0)
            throw new Exception("No words");

        var result = Process.ExtractOne(text, words, (s) => s);

        Debug.Log(text);
        Debug.Log($"{result.Value}: {result.Score}");

        if (result.Score >= _itemScoreThreshold && result.Index >= 0 && result.Index < _collectedItems.Count)
        {
            _collectedItems[result.Index].RestoreSocketedItem(_itemAttachPoint);
            OnItemFound?.Invoke(_collectedItems[result.Index]);
        }
        else
            throw new Exception("No item found");
    }

    /// <summary>
    /// Adds item to collected list.
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(GrabbableObject item)
    {
        if(!_collectedItems.Contains(item))
            _collectedItems.Add(item);
    }

    /// <summary>
    /// Removes item from collected list.
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(GrabbableObject item)
    {
        _collectedItems.Remove(item);
    }

    private void OnDisable()
    {
        _socket.selectEntered.RemoveAllListeners();
        _speechController.onTranscribed -= InstantiateItemByTitle;
    }
}
