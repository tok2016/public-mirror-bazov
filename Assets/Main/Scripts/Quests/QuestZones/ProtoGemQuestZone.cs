using UnityEngine;

public class ProtoGemQuestZone : QuestZone<ProtoGemQuest>
{
    [SerializeField] private ProtoGemProps _props;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            _quest.Enter(_props);
    }
}
