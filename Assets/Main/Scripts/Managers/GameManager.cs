using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TeleportationProvider _teleportationProvider;
    [SerializeField] private NearFarInteractor _leftController, _rightController;
    [SerializeField] private InputActionReference _skipAction;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        QuestManager.Runner = this;
    }

    private void OnEnable()
    {
        _leftController.selectEntered.AddListener(QuestManager.GrabQuestItem);
        _rightController.selectEntered.AddListener(QuestManager.GrabQuestItem);

        _leftController.selectExited.AddListener(QuestManager.LetGoQuestItem);
        _rightController.selectExited.AddListener(QuestManager.LetGoQuestItem);

        _teleportationProvider.locomotionStarted += QuestManager.StartQuestTeleportation;
        _teleportationProvider.locomotionEnded += QuestManager.EndQuestTeleportation;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (_skipAction.action.WasPressedThisFrame())
            QuestManager.SkipCutscene();
    }

    private void OnDisable()
    {
        _leftController.selectEntered.RemoveListener(QuestManager.GrabQuestItem);
        _rightController.selectEntered.RemoveListener(QuestManager.GrabQuestItem);

        _leftController.selectExited.RemoveListener(QuestManager.LetGoQuestItem);
        _rightController.selectExited.RemoveListener(QuestManager.LetGoQuestItem);

        _teleportationProvider.locomotionStarted -= QuestManager.StartQuestTeleportation;
        _teleportationProvider.locomotionEnded -= QuestManager.EndQuestTeleportation;
    }
}
