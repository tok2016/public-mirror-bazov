using UnityEngine;

[CreateAssetMenu(fileName = "DefaultQuestData", menuName = "Scriptable Objects/QuestData/Default")]
public class QuestData : ScriptableObject
{
    [field: SerializeField]
    public QuestData[] Next { get; protected set; }

    [field: SerializeField]
    public string Id { get; protected set; }

    [field: SerializeField]
    public string Name { get; protected set; }

    [field: SerializeField, TextArea] public string Description { get; protected set; }
}
