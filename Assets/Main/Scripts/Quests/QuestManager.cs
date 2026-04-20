using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class QuestManager : MonoBehaviour
{
    public QuestBase Current {  get; private set; }
    [SerializeField] private QuestBase _firstQuest;
    public UnityEvent onQuestComplete;
    public UnityEvent onQuestStart;

    public static QuestManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Current = _firstQuest;
    }

    private void Update()
    {
        Current?.Update();
    }

    public void StartQuest()
    {
        onQuestStart?.Invoke();
    }

    public void OnCheckQuest(SelectEnterEventArgs args)
    {
        Current?.Check(args);
    }

    public void OnItemGrab(SelectEnterEventArgs args)
    {
        Current?.OnGrab(args);
    }

    public void OnItemLettingGo(SelectExitEventArgs args)
    {
        Current?.OnLettingGo(args);
    }

    public void CompleteQuest()
    {
        Current = Current.Next;
        onQuestComplete?.Invoke();
    }
}
