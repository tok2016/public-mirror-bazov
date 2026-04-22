using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public abstract class Quest : MonoBehaviour
{
    [SerializeField] protected QuestData _data;
    [field: SerializeField] public Quest Next {  get; private set; }
    private QuestState _state = QuestState.Locked;

    public UnityEvent onFirstEnter;
    public UnityEvent onEnterRepeat;
    public UnityEvent onExit;
    public UnityEvent onComplete;

    protected virtual void Awake()
    {
        gameObject.SetActive(false);
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    public void Unlock()
    {
        _state = QuestState.Available;
        gameObject.SetActive(true);
    }

    public virtual void Enter()
    {
        QuestManager.Instance.StartQuest();
        if (_state == QuestState.Locked)
            onFirstEnter.Invoke();
        else
            onEnterRepeat.Invoke();
        _state = QuestState.InProgress;
        Debug.Log(_data.StartPhraseText);
    }

    public virtual void Exit()
    {
        _state = QuestState.Available;
        onExit.Invoke();
    }

    public virtual void Complete()
    {
        QuestManager.Instance.CompleteQuest();
        _state = QuestState.Completed;
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

    internal virtual void OnTeleport(TeleportingEventArgs args) { }
}
