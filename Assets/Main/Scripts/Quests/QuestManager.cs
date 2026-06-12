using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public static class QuestManager
{
    private static Quest _current;
    private static Quest s_Current 
    { 
        get => _current;
        set
        {
            _current = value;
        }
    }

    private static Queue<Quest> _quests = new Queue<Quest>();
    private static bool _IsOccupied = false;
    private static MonoBehaviour _runner;
    public static MonoBehaviour Runner { 
        get => _runner; 
        set 
        {
            if (!_runner)
                _runner = value;
            else
                throw new System.Exception("Can'r reassign runner");
        } 
    }

    public static void EnqueueQuest(Quest quest)
    {
        if (s_Current == null || s_Current == quest)
        {
            quest.Unlock();
            quest.Enter();
        }   
        else
            _quests.Enqueue(quest);
    }

    public static void StartQuest(Quest quest)
    {
        quest.gameObject.SetActive(true);
        s_Current = quest;
        Runner.StartCoroutine(WaitForQuestStart(quest));
    }

    public static void StopQuest(Quest quest)
    {
        quest.gameObject.SetActive(false);

        if (quest == s_Current)
            s_Current = null;

        if (_quests.Count == 0) return;
        else if (_quests.Count > 1)
        {
            var tempQueue = new Queue<Quest>();
            while (_quests.Count > 0)
            {
                var currentQuest = _quests.Dequeue();
                if (currentQuest != quest)
                    tempQueue.Enqueue(currentQuest);
            }

            _quests = tempQueue;
        }

        s_Current = _quests.Dequeue();
        s_Current.Enter();
    }

    public static void CompleteQuest(Quest quest)
    {
        Runner.StartCoroutine(WaitForQuestComplete(quest));
    }

    public static void SkipCutscene()
    {
        s_Current?.SkipCutscene();
    }

    private static System.Collections.IEnumerator WaitForQuestStart(Quest quest)
    {
        _IsOccupied = true;
        if (quest.State == QuestState.Available)
            yield return quest.WaitBeforeStart();
        _IsOccupied = false;
    }

    private static System.Collections.IEnumerator WaitForQuestComplete(Quest quest)
    {
        while(_IsOccupied)
            yield return null;

        yield return quest.WaitAfterComplete();
        quest.Next?.Unlock();
        StopQuest(quest);
    }

    public static void GrabQuestItem(SelectEnterEventArgs args)
    {
        s_Current?.OnItemGrab(args);
    }

    public static void LetGoQuestItem(SelectExitEventArgs args)
    {
        s_Current?.OnItemLettingGo(args);
    }

    public static void StartQuestTeleportation(LocomotionProvider provider)
    {
        s_Current?.OnTeleportStart(provider);
    }

    public static void EndQuestTeleportation(LocomotionProvider provider)
    {
        s_Current?.OnTeleportEnd(provider);
    }
}
