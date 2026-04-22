using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField, TextArea] public string Description { get; private set; }
    [field: SerializeField] public AudioSource StartPhrase { get; private set; }
    [field: SerializeField, TextArea] public string StartPhraseText { get; private set; }
    [field: SerializeField] public AudioSource EndPhrase { get; private set; }
    [field: SerializeField, TextArea] public string EndPhraseText { get; private set; }
}
