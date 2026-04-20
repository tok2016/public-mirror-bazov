using UnityEngine;
using UnityEngine.Events;

public class QuestZone<QuestType> : MonoBehaviour where QuestType : QuestBase
{
    [SerializeField] protected QuestType _quest;

    public UnityEvent onFirstEnter;
    public UnityEvent onEnterRepeat;
    public UnityEvent onExit;
    public UnityEvent onComplete;

    private void OnEnable()
    {
        _quest.onFirstEnter.AddListener(OnFirstEnter);
        _quest.onEnterRepeat.AddListener(OnRepeatEnter);
        _quest.onExit.AddListener(OnExit);
        _quest.onComplete.AddListener(OnComplete);
    }

    private void OnFirstEnter()
    {
        onFirstEnter.Invoke();
    }

    private void OnRepeatEnter()
    {
        onEnterRepeat.Invoke();
    }

    private void OnExit()
    {
        onExit.Invoke();
    }

    private void OnComplete() 
    { 
        onComplete.Invoke(); 
    }
}
