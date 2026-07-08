using System.Linq;
using UnityEngine;

/// <summary>
/// Manages quests life cycle in one area. 
/// </summary>
public class QuestZone : MonoBehaviour
{
    [SerializeField] private Quest[] _quests;
    [SerializeField] private CombinedTrigger _combinedTrigger;

    private void OnEnable()
    {
        _combinedTrigger.OnTriggerGroupEnter += EnterQuestZone;
        _combinedTrigger.OnTriggerGroupExit += ExitQuestZone;
    }

    /// <summary>
    /// Enter the first incomplete quest when player enters the zone.
    /// </summary>
    /// <param name="other">Collider entered the zone</param>
    private void EnterQuestZone(Collider other)
    {
        if(other.tag == "Player")
        {
            var current = GetFirstIncompleteQuest();
            current?.Enter();
        }
    }

    /// <summary>
    /// Exits the first incomplete quest when player exits the zone 
    /// </summary>
    /// <param name="other">Collider exited the zone</param>
    private void ExitQuestZone(Collider other)
    {
        if(other.tag == "Player")
        {
            var current = GetFirstIncompleteQuest();
            current?.Exit();
        }
    }

    /// <summary>
    /// Finds first incomplete quest in the list. If every quest in the zone was completed, returns the last one.
    /// </summary>
    /// <returns>Instance of first incomplete or last quest.</returns>
    private Quest GetFirstIncompleteQuest()
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
