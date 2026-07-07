using UnityEngine;

/// <summary>
/// Contains data about dictionary word.
/// </summary>
[CreateAssetMenu(fileName = "Word", menuName = "Scriptable Objects/WordData")]
public class WordData : ScriptableObject
{
    /// <value>
    /// The word itself, a terming.
    /// </value>
    [field: SerializeField] public string Title {  get; private set; }

    /// <summary>
    /// Definition of the word.
    /// </summary>
    [field: SerializeField, TextArea] public string Description { get; private set; }
}
