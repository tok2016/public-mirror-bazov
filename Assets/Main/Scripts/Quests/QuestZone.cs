using System.Linq;
using UnityEngine;

public class QuestZone : MonoBehaviour
{
    [SerializeField] private Quest[] _quests;
    [SerializeField] private CombinedTrigger _combinedTrigger;

    private void OnEnable()
    {
        _combinedTrigger.OnTriggerGroupEnter += EnterQuestZone;
        _combinedTrigger.OnTriggerGroupExit += ExitQuestZone;
    }

    private void EnterQuestZone(Collider other)
    {
        if(other.tag == "Player")
        {
            var current = GetLastIncompleteQuest();
            current?.Enter();
        }
    }

    private void ExitQuestZone(Collider other)
    {
        if(other.tag == "Player")
        {
            var current = GetLastIncompleteQuest();
            current?.Exit();
        }
    }

    private Quest GetLastIncompleteQuest()
    {
        try
        {
            return _quests.First((quest) => quest.State != QuestState.Completed);
        }
        catch
        {
            return _quests.Last();
        }
    }

    private void OnDisable()
    {
        _combinedTrigger.OnTriggerGroupEnter -= EnterQuestZone;
        _combinedTrigger.OnTriggerGroupExit -= ExitQuestZone;
    }
}
