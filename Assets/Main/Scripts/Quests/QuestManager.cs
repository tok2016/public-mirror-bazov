using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class QuestManager : MonoBehaviour
{
    private Quest _current;
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

        _current = _firstQuest;
    }

    private void OnEnable()
    {
        _teleportationProvider.locomotionStarted += OnTeleportStart;
        _teleportationProvider.locomotionEnded += OnTeleportEnd;
    }

    private void Start()
    {
        _current?.Unlock();
        _current?.Enter();
    }

    public void StartQuest(Quest quest)
    {
        if(_current != quest)
            _current = quest;

        onQuestStart?.Invoke();
    }

    public void CompleteQuest()
    {
        if(_current && _current.Next)
            _current.Next.Unlock();
        onQuestComplete?.Invoke();
    }

    public bool IsInActiveZone(Transform item) => _current?.IsItemInActiveZone(item) ?? true;

    public void ReturnToActiveZone(Transform item)
    {
        _current?.ReturnToActiveZone(item);
    }

    public void Check(SelectEnterEventArgs args)
    {
        _current?.Check(args);
    }

    public void Check(SelectExitEventArgs args)
    {
        _current?.Check(args);
    }

    public void Check(TeleportingEventArgs args)
    {
        _current?.Check(args);
    }

    public void Check()
    {
        _current?.Check();
    }

    public void OnItemGrab(SelectEnterEventArgs args)
    {
        _current?.OnItemGrab(args);
    }

    public void OnItemLettingGo(SelectExitEventArgs args)
    {
        _current?.OnItemLettingGo(args);
    }

    public void OnTeleportStart(LocomotionProvider provider)
    {
        _current?.OnTeleportStart(provider);
    }

    public void OnTeleportEnd(LocomotionProvider provider)
    {
        _current?.OnTeleportEnd(provider);
    }

    public void OnTeleport(TeleportingEventArgs args)
    {
        _current?.OnTeleport(args);
    }

    private void OnDisable()
    {
        _teleportationProvider.locomotionStarted -= OnTeleportStart;
        _teleportationProvider.locomotionEnded -= OnTeleportEnd;
    }
}
