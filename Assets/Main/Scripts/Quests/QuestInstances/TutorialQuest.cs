using UnityEngine;

public abstract class TutorialQuest : Quest
{
    [Header("Hints")]
    [SerializeField] protected HintObject[] _hintObjects;
    [SerializeField] protected ActionsVisualizer _leftController, _rightController;

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
