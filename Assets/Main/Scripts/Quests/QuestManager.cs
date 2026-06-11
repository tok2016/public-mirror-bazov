using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public static class QuestManager
{
    private static Quest _current;
    private static Quest Current 
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
        if (Current == null || Current == quest)
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
        Current = quest;
        Runner.StartCoroutine(WaitForQuestStart(quest));
    }

    public static void StopQuest(Quest quest)
    {
        quest.gameObject.SetActive(false);

        if (quest == Current)
            Current = null;

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

        Current = _quests.Dequeue();
        Current.Enter();
    }

    public static void CompleteQuest(Quest quest)
    {
        Runner.StartCoroutine(WaitForQuestComplete(quest));
    }

    public static void SkipCutscene()
    {
        Current?.SkipCutscene();
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

    public static bool IsInActiveZone(Transform item) => Current?.IsItemInActiveZone(item) ?? true;

    public static void ReturnToActiveZone(Transform item)
    {
        Current?.ReturnToActiveZone(item);
    }

    public static void GrabQuestItem(SelectEnterEventArgs args)
    {
        Current?.OnItemGrab(args);
    }

    public static void LetGoQuestItem(SelectExitEventArgs args)
    {
        Current?.OnItemLettingGo(args);
    }

    public static void StartQuestTeleportation(LocomotionProvider provider)
    {
        Current?.OnTeleportStart(provider);
    }

    public static void EndQuestTeleportation(LocomotionProvider provider)
    {
        Current?.OnTeleportEnd(provider);
    }
}
