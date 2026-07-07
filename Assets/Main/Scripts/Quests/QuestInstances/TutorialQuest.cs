using UnityEngine;

/// <summary>
/// Base class for all tutorial quests. Manages hints of quest.
/// </summary>
public abstract class TutorialQuest : Quest
{
    [Header("Hints")]
    [SerializeField] protected HintObject[] _hintObjects;
    [SerializeField] protected ActionsVisualizer _leftController, _rightController;

    /// <summary>
    /// Enables or disables hints on controller.
    /// </summary>
    /// <param name="enable">Whether to enable or disable hints</param>
    protected abstract void ToggleControllersHint(bool enable);

    protected override void Activate()
    {
        foreach (var hintObject in _hintObjects)
            hintObject.ToggleOutline(true);
        ToggleControllersHint(true);
    }

    protected override void Check()
    {
        Complete();
    }

    protected override void Deactivate()
    {
        foreach (var hintObject in _hintObjects)
            hintObject.ToggleOutline(false);
        ToggleControllersHint(false);
    }

    protected override void Stop()
    {
        Deactivate();
    }
}
