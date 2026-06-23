using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    [SerializeField] private TeleportPad _padToActivate;
    [SerializeField] private InputActionReference _startAction;
    [SerializeField] private ActionsVisualizer[] _visualizers;
    [SerializeField] private Button _quitButton;

    private void OnEnable()
    {
        _startAction.action.performed += StartQuest;
        _quitButton.onClick.AddListener(Quit);
    }

    void Start()
    {
        foreach (var visualizer in _visualizers)
            visualizer.WarnAboutPause(true);
    }

    public void StartQuest(InputAction.CallbackContext context)
    {
        _padToActivate.Activate();
        _padToActivate.EnterThePad();
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnDisable()
    {
        _startAction.action.performed -= StartQuest;
        _quitButton.onClick.RemoveListener(Quit);
    }
}
