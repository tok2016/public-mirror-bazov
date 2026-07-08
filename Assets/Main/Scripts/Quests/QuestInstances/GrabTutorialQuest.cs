using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Checks for grab tutorial quest completion.
/// </summary>
public class GrabTutorialQuest : TutorialQuest
{
    [SerializeField] private XRGrabInteractable _targetObject;

    private void OnEnable()
    {
        _targetObject.selectEntered.AddListener(OnTargetGrab);
    }

    /// <summary>
    /// Checks if player grabbed the correct item.
    /// </summary>
    /// <param name="args"></param>
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
