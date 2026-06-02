using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class QuestZone : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onExit;

    [SerializeField] private Quest[] _quests;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            onEnter.Invoke();
            var current = GetLastIncompleteQuest();
            current?.Enter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            onExit.Invoke();
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
}
