using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class QuestManager : MonoBehaviour
{
    public Quest Current {  get; private set; }
    [SerializeField] private Quest _firstQuest;
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

    private void Start()
    {
        Current?.Enter();
    }

    public void StartQuest()
    {
        onQuestStart?.Invoke();
    }

    public void CompleteQuest()
    {
        Current = Current?.Next;
        Current?.Unlock();
        onQuestComplete?.Invoke();
    }

    public void Check(SelectEnterEventArgs args)
    {
        Current?.Check(args);
    }

    public void Check(SelectExitEventArgs args)
    {
        Current?.Check(args);
    }

    public void Check(TeleportingEventArgs args)
    {
        Current?.Check(args);
    }

    public void Check()
    {
        Current?.Check();
    }

    public void OnItemGrab(SelectEnterEventArgs args)
    {
        Current?.OnItemGrab(args);
    }

    public void OnItemLettingGo(SelectExitEventArgs args)
    {
        Current?.OnItemLettingGo(args);
    }

    public void OnTeleport(TeleportingEventArgs args)
    {
        Current?.OnTeleport(args);
    }
}
