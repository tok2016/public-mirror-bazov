using UnityEngine;

public abstract class Quest<PropsType> : QuestBase
{
    public virtual void Enter(PropsType props)
    {
        State = QuestState.InProgress;
        if (State == QuestState.Locked)
        {
            QuestManager.Instance.StartQuest();
            onFirstEnter?.Invoke();
        }
        else
            onEnterRepeat?.Invoke();

        Debug.Log(StartPhrase);
    }
}
