using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public abstract class Quest : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] protected QuestData _data;
    [field: SerializeField] public Quest Next {  get; private set; }
    [SerializeField] private ItemActiveZone _activeZone;
    [SerializeField] private RespawningItem[] _importantItems;

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

        QuestManager.StartQuest(this);
        if (State == QuestState.Available)
        {
            onFirstEnter.Invoke();
            DialogueManager.PlayLine(_data.StartPhrase);
        } 
        else
            onEnterRepeat.Invoke();

        foreach (var item in _importantItems)
            item.enabled = true;

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
        foreach (var item in _importantItems)
            item.enabled = false;

        QuestManager.CompleteQuest();
        State = QuestState.Completed;
        DialogueManager.PlayLine(_data.EndPhrase);
        onComplete.Invoke();
        gameObject.SetActive(false);
    }

    public bool IsItemInActiveZone(Transform item) => _activeZone.IsItemInActiveZone(item);

    public virtual void ReturnToActiveZone(Transform item)
    {
        _activeZone.ReturnToActiveZone(item);
    }

    protected abstract void Check();

    internal virtual void OnItemGrab(SelectEnterEventArgs args) { }

    internal virtual void OnItemLettingGo(SelectExitEventArgs args) { }

    internal virtual void OnTeleportStart(LocomotionProvider provider) { }

    internal virtual void OnTeleportEnd(LocomotionProvider provider) { }
}
