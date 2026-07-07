using UnityEngine;

/// <summary>
/// Contains data about quest.
/// </summary>
[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    /// <value>
    /// Quest name or title.
    /// </value>
    [field: SerializeField] public string Name { get; private set; }

    /// <value>
    /// Quest description.
    /// </value>
    [field: SerializeField, TextArea] public string Description { get; private set; }

    /// <value>
    /// The message when quest was completed.
    /// </value>
    [field: SerializeField, TextArea] public string CompleteMessage {  get; private set; }
}
