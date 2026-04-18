using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class Quest : MonoBehaviour
{
    [SerializeField] protected QuestData _data;
    public UnityEvent onFirstEnter;
    public UnityEvent onEnterRepeat;
    public UnityEvent onExit;
    public UnityEvent onComplete;

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
            onFirstEnter.Invoke();
        }
        else
            onEnterRepeat.Invoke();

        Debug.Log("Enter");
    }

    public virtual void Exit()
    {
        onExit.Invoke();
        Debug.Log("Exit");
    }

    public abstract void Check(SelectEnterEventArgs args);

    public virtual void Complete()
    {
        onComplete.Invoke();
        Debug.Log("Complete");
        enabled = false;
    }
}
