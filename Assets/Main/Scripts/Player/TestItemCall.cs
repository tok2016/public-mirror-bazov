using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct TestItemCallOption
{
    public CollectableItem item;
    public InputActionReference action;
}

/// <summary>
/// Instantly makes collected items available by button press to get them with no need ti call them in mictophone.
/// </summary>
public class TestItemCall : MonoBehaviour
{
    public static TestItemCall Instance { get; private set; }
    [SerializeField] private TestItemCallOption[] _itemsCallOptions;
    [SerializeField] private Transform _attachPoint;
    private CollectableItem _currentItem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        CheckCallInput();
    }

    /// <summary>
    /// Checks if any button was pressed and makes corresponding item accessible.
    /// </summary>
    public void CheckCallInput()
    {
        foreach(var option in _itemsCallOptions)
        {
            if(option.action.action.WasPressedThisFrame())
            {
                _currentItem?.TransformSocketedItem();
                option.item.RestoreSocketedItem(_attachPoint);
                _currentItem = option.item;
                return;
            }
        }
    }
}
