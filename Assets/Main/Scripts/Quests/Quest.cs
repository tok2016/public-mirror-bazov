using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class Quest<T> : MonoBehaviour where T : QuestData
{
    [SerializeField] private T _data;
    public UnityEvent onFirstEnter;
    public UnityEvent onEnterRepeat;
    public UnityEvent onExit;
    public UnityEvent onComplete;

    [SerializeField] private TextMeshProUGUI _title;

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

        _title.text = _data.Name;
        _title.fontStyle = FontStyles.Normal;
        Debug.Log("Enter");
    }

    public virtual void Exit()
    {
        onExit.Invoke();
        Debug.Log("Exit");
    }

    public virtual void Complete()
    {
        if (_data.Next.Length == 0)
        {
            _title.text = " вест завершен";
            _title.color = Color.green;
        }
        else
            _title.fontStyle = FontStyles.Strikethrough;

        onComplete.Invoke();
        Debug.Log("Complete");
    }
}
