using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public abstract class Quest : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] protected QuestData _data;
    [field: SerializeField] public Quest Next {  get; private set; }
    public QuestState State { get; protected set; } = QuestState.Locked;

    [Header("Enter")]
    public UnityEvent onFirstEnter;
    public UnityEvent onEnterRepeat;
    public UnityEvent onEnterAfterComplete;

    [Header("Exit")]
    public UnityEvent onExit;
    public UnityEvent onExitAfterComplete;

    [Header("Complete")]
    public UnityEvent onComplete;

    protected virtual void Awake()
    {
        gameObject.SetActive(false);
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    public void Unlock()
    {
        State = QuestState.Available;
        gameObject.SetActive(true);
    }

    public virtual void Enter()
    {
        if (State == QuestState.Completed)
        {
            onEnterAfterComplete.Invoke();
            return;
        }
        else if (State == QuestState.Locked)
            Unlock();


        QuestManager.Instance.StartQuest(this);
        if (State == QuestState.Available)
        {
            onFirstEnter.Invoke();
            Debug.Log(_data.StartPhraseText);
        } 
        else
            onEnterRepeat.Invoke();

        State = QuestState.InProgress;
    }

    public virtual void Exit()
    {
        if(State == QuestState.Completed)
        {
            onExitAfterComplete.Invoke();
            return;
        }

        State = QuestState.Visited;
        onExit.Invoke();
    }

    public virtual void Complete()
    {
        QuestManager.Instance.CompleteQuest();
        State = QuestState.Completed;
        Debug.Log(_data.EndPhraseText);
        onComplete.Invoke();
        gameObject.SetActive(false);
    }

    internal virtual void Check(SelectEnterEventArgs args)
    {
        Complete();
    }

    internal virtual void Check(SelectExitEventArgs args)
    {
        Complete();
    }

    internal virtual void Check(TeleportingEventArgs args)
    {
        Complete();
    }

    internal virtual void Check()
    {
        Complete();
    }

    internal virtual void OnItemGrab(SelectEnterEventArgs args) { }

    internal virtual void OnItemLettingGo(SelectExitEventArgs args) { }

    internal virtual void OnTeleportStart(LocomotionProvider provider) { }

    internal virtual void OnTeleportEnd(LocomotionProvider provider) { }

    internal virtual void OnTeleport(TeleportingEventArgs args) { }
}
