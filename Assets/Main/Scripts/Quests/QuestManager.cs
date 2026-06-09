using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public static class QuestManager
{
    private static Quest _current;

    public static void StartQuest(Quest quest)
    {
        if (_current != quest)
            _current = quest;
    }

    public static void CompleteQuest()
    {
        if (_current && _current.Next)
            _current.Next.Unlock();
    }

    public static bool IsInActiveZone(Transform item) => _current?.IsItemInActiveZone(item) ?? true;

    public static void ReturnToActiveZone(Transform item)
    {
        _current?.ReturnToActiveZone(item);
    }

    public static void GrabQuestItem(SelectEnterEventArgs args)
    {
        _current?.OnItemGrab(args);
    }

    public static void LetGoQuestItem(SelectExitEventArgs args)
    {
        _current?.OnItemLettingGo(args);
    }

    public static void StartQuestTeleportation(LocomotionProvider provider)
    {
        _current?.OnTeleportStart(provider);
    }

    public static void EndQuestTeleportation(LocomotionProvider provider)
    {
        _current?.OnTeleportEnd(provider);
    }
}
