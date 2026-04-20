using UnityEngine;

public class BagQuestZone : QuestZone<BagQuest>
{
    void Start()
    {
        _quest.Enter(0);
    }
}
