using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField, TextArea] public string Description { get; private set; }
    [field: SerializeField] public DialogueLine StartPhrase { get; private set; }
    [field: SerializeField] public TimelineAsset StartCutscene { get; private set; }
    [field: SerializeField] public DialogueLine EndPhrase { get; private set; }
    [field: SerializeField] public TimelineAsset EndCutscene { get; private set; }
}
