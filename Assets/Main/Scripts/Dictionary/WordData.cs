using UnityEngine;

[CreateAssetMenu(fileName = "Word", menuName = "Scriptable Objects/WordData")]
public class WordData : ScriptableObject
{
    [field: SerializeField] public string Title {  get; private set; }
    [field: SerializeField, TextArea] public string Description { get; private set; }
}
