using UnityEngine;

/// <summary>
/// Contains data about grabbable item
/// </summary>
[CreateAssetMenu(fileName = "CollectableItemData", menuName = "Scriptable Objects/CollectableItemData")]
public class CollectableItemData : ScriptableObject
{
    /// <value>
    /// Name of the item.
    /// </value>
    [field: SerializeField] public string Name {  get; private set; }

    /// <value>
    /// Line to comment or describe item.
    /// </value>
    [field: SerializeField] public DialogueLine DialogueLine { get; private set; }
}
