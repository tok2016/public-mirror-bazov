using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class QuestManager : MonoBehaviour
{
    public Quest Current {  get; private set; }
    [SerializeField] private Quest _firstQuest;
    [SerializeField] private TeleportationProvider _teleportationProvider;
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

    private void OnEnable()
    {
        _teleportationProvider.locomotionStarted += OnTeleportStart;
        _teleportationProvider.locomotionEnded += OnTeleportEnd;
    }

    private void Start()
    {
        Current?.Unlock();
        Current?.Enter();
    }

    public void StartQuest(Quest quest)
    {
        if(Current != quest)
            Current = quest;

        onQuestStart?.Invoke();
    }

    public void CompleteQuest()
    {
        if(Current && Current.Next)
            Current.Next.Unlock();
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

    public void OnTeleportStart(LocomotionProvider provider)
    {
        Current?.OnTeleportStart(provider);
    }

    public void OnTeleportEnd(LocomotionProvider provider)
    {
        Current?.OnTeleportEnd(provider);
    }

    public void OnTeleport(TeleportingEventArgs args)
    {
        Current?.OnTeleport(args);
    }

    private void OnDisable()
    {
        _teleportationProvider.locomotionStarted -= OnTeleportStart;
        _teleportationProvider.locomotionEnded -= OnTeleportEnd;
    }
}
