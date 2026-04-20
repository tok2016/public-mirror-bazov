using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class QuestBase : ScriptableObject
{
    [field: SerializeField] public QuestBase Next { get; protected set; }
    [field: SerializeField] public string Id { get; protected set; }
    [field: SerializeField] public string Name { get; protected set; }
    [field: SerializeField, TextArea] public string Description { get; protected set; }
    [field: SerializeField, TextArea] public string StartPhrase { get; protected set; }
    [field: SerializeField, TextArea] public string EndPhrase { get; protected set; }
    public QuestState State { get; protected set; } = QuestState.Locked;

    public UnityEvent onFirstEnter;
    public UnityEvent onEnterRepeat;
    public UnityEvent onExit;
    public UnityEvent onComplete;

    public virtual void Update() { }

    public virtual void Exit()
    {
        State = QuestState.Available;
        onExit.Invoke();
    }

    public abstract void Check(SelectEnterEventArgs args);

    public virtual void OnGrab(SelectEnterEventArgs args) { }

    public virtual void OnLettingGo(SelectExitEventArgs args) { }

    public virtual void Complete()
    {
        State = QuestState.Completed;
        QuestManager.Instance.CompleteQuest();
        onComplete.Invoke();
        Debug.Log(EndPhrase);
    }
}
