using UnityEngine;

[CreateAssetMenu(fileName = "CollectableItemData", menuName = "Scriptable Objects/CollectableItemData")]
public class CollectableItemData : ScriptableObject
{
    [field: SerializeField] public string Name {  get; private set; }
    [field: SerializeField, TextArea] public string Hint { get; private set; }
    [field: SerializeField, TextArea] public string Commentary { get; private set; }
    [field: SerializeField] public AudioClip AudioCommentary { get; private set; }
    [field: SerializeField] public WordData Word { get; private set; }
}
