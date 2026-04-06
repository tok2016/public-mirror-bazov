using System;
using UnityEngine;

public abstract class Quest<T> : MonoBehaviour where T : QuestData
{
    [SerializeField] private T _data;
    public event Action onFirstEnter;
    public event Action onEnterRepeat;
    public event Action onExit;
    public event Action onComplete;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    public virtual void Enter()
    {
        var state = QuestManager.Instance.GetQuestState(_data);
        if (state == null)
            throw new Exception("Quest doesn't exist");
        else if(state == QuestState.Available)
        {
            QuestManager.Instance.StartQuest(_data);
            onFirstEnter?.Invoke();
        }
        else
            onEnterRepeat?.Invoke();

        Debug.Log("Enter");
    }

    public virtual void Exit()
    {
        onExit?.Invoke();
        Debug.Log("Exit");
    }

    public virtual void Complete()
    {
        onComplete?.Invoke();
        Debug.Log("Complete");
    }

    public abstract void Check();
}
