using UnityEngine;

public class CollectingQuestZone : QuestZone<CollectingQuest>
{
    [SerializeField] private CollectingQuestProps _props;

    public void EnterQuest()
    {
        _quest.Enter(_props);
    }
}
