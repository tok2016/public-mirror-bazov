using System;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public class Pause : MonoBehaviour
{
    [SerializeField] private InputActionReference _pauseAction;
    [SerializeField] private LocomotionProvider _locomotionProvider;
    [SerializeField] private XRBaseInteractor[] _teleportInteractors;
    [SerializeField] private XROrigin _origin;
    [SerializeField] private GameObject _pausePlace;
    [SerializeField] private Transform _pauseAttachPoint;
    public static bool IsPaused { get; private set; } = false;
    public static event Action onPause, onContinue;
    private Vector3 _prevPosition;

    [Header("UI")]
    [SerializeField] private GameObject _confirmMenu;
    [SerializeField] private Button _quitButton, _confirmButton, _cancelButton;
    [SerializeField] private TextMeshProUGUI _questTitle, _questDescription;

    private void OnEnable()
    {
        _pauseAction.action.performed += ToggleActive;
        _quitButton.onClick.AddListener(OpenConfirmMenu);
        _confirmButton.onClick.AddListener(Quit);
        _cancelButton.onClick.AddListener(CloseConfirmMenu);
        ResetQuestInfo();
    }

    public void ToggleActive()
    {
        IsPaused = !IsPaused;
        _pausePlace.gameObject.SetActive(IsPaused);
        _locomotionProvider.enabled = !IsPaused;

        foreach(var interactor  in _teleportInteractors)
            interactor.enabled = !IsPaused;

        if (IsPaused)
        {
            _prevPosition = _origin.transform.position;
            _origin.transform.position = _pauseAttachPoint.position;
            _origin.transform.rotation = _pauseAttachPoint.rotation;
            onPause?.Invoke();
        }
        else
        {
            _origin.transform.position = _prevPosition;
            onContinue?.Invoke();
        }
    }

    public void ToggleActive(InputAction.CallbackContext context)
    {
        ToggleActive();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenConfirmMenu()
    {
        _confirmMenu.SetActive(true);
    }

    public void CloseConfirmMenu()
    {
        _confirmMenu.SetActive(false);
    }

    public void ShowQuestInfo(QuestData questData, string state, bool isCompleted)
    {
        _questTitle.text = $"{state}: {questData.Name}";
        _questDescription.text = isCompleted ? questData.CompleteMessage : questData.Description; 
    }

    public void ResetQuestInfo()
    {
        _questTitle.text = "Ќет доступных заданий";
        _questDescription.text = "ѕроходите дальше по квесту, чтобы получить больше заданий";
    }

    private void OnDisable()
    {
        _pauseAction.action.performed -= ToggleActive;
        _pauseAction.action.performed -= ToggleActive;
        _quitButton.onClick.RemoveListener(OpenConfirmMenu);
        _confirmButton.onClick.RemoveListener(Quit);
        _cancelButton.onClick.RemoveListener(CloseConfirmMenu);
    }
}
