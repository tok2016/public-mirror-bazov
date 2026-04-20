using UnityEngine;

public class ProtoCollectingQuestZone : QuestZone<ProtoCollectingQuest>
{
    [SerializeField] private ProtoCollectingProps _props;

    private void Start()
    {
        _quest.Enter(_props);
    }
}
