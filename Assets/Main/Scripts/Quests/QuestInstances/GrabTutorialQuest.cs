using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabTutorialQuest : TutorialQuest
{
    [SerializeField] private XRGrabInteractable _targetObject;

    private void OnEnable()
    {
        _targetObject.selectEntered.AddListener(OnTargetGrab);
    }

    private void OnTargetGrab(SelectEnterEventArgs args)
    {
        Check();
    }

    private void OnDisable()
    {
        _targetObject.selectEntered.RemoveListener(OnTargetGrab);
    }

    protected override void ToggleControllersHint(bool enable)
    {
        
    }
}
