using UnityEngine;

/// <summary>
/// Visualizes input actions by changing pose of the hand.
/// </summary>
public class HandVisualizer : ActionsVisualizer
{
    [SerializeField] private Animator _handAnimator;
    private VisualizerState _prevState;

    /// <summary>
    /// Shows that grab action is disabled by setting pose to default.
    /// </summary>
    /// <param name="enable">If true, sets pose to default.</param>
    public override void DisableGrab(bool enable)
    {
        if(enable)
        {
            _handAnimator?.SetBool("ReadyToGrab", false);
            _handAnimator?.SetBool("IsGrabbing", false);
        }
    }

    /// <summary>
    /// Shows that poke action is disabled by setting pose to default. 
    /// </summary>
    /// <param name="enable">If true, sets pose to default.</param>
    public override void DisablePoke(bool enable)
    {
        if (enable)
            _handAnimator?.SetBool("IsPoking", false);
    }

    /// <summary>
    /// Shows that record action is disabled by setting pose to default. 
    /// </summary>
    /// <param name="enable">If true, sets pose to default.</param>
    public override void DisableRecording(bool enable)
    {
        if (enable)
        {
            _handAnimator?.SetBool("ReadyToRecord", false);
            _handAnimator?.SetBool("IsRecording", false);
        }
    }

    /// <summary>
    /// Shows that teleport action is disabled by setting pose to default. 
    /// </summary>
    /// <param name="enable">If true, sets pose to default.</param>
    public override void DisableTeleport(bool enable)
    {
        DisablePoke(enable);
    }


    /// <summary>
    /// Sets hand pose to grabbing state.
    /// </summary>
    /// <param name="enable">If true, sets pose to grabbing state.</param>
    public override void ShowGrab(bool enable)
    {
        _handAnimator?.SetBool("IsGrabbing", enable);
    }

    /// <summary>
    /// Sets hand pose to index bend state.
    /// </summary>
    /// <param name="enable">If true, sets pose to index bend state.</param>
    public override void ShowPoke(bool enable)
    {
        if (enable)
            _handAnimator?.SetTrigger("Poke");
    }

    /// <summary>
    /// Sets hand pose to recording state.
    /// </summary>
    /// <param name="enable">If true, sets pose to recording state.</param>
    public override void ShowRecording(bool enable)
    {
        _handAnimator?.SetBool("IsRecording", enable);
    }

    /// <summary>
    /// Sets hand pose to pointing state.
    /// </summary>
    /// <param name="enable">If true, sets pose to pointing state.</param>
    public override void ShowTeleport(bool enable)
    {
        WarnAboutPoke(enable);
    }


    /// <summary>
    /// Sets hand pose to open state as it's ready to grab.
    /// </summary>
    /// <param name="enable">If true, sets pose to open state.</param>
    public override void WarnAboutGrab(bool enable)
    {
        _handAnimator?.SetBool("ReadyToGrab", enable);
    }

    /// <summary>
    /// Sets hand pose to pointing state.
    /// </summary>
    /// <param name="enable">If true, sets pose to pointing state.</param>
    public override void WarnAboutPoke(bool enable)
    {
        _handAnimator?.SetBool("IsPoking", enable);
    }

    /// <summary>
    /// Sets hand pose to ready to record state.
    /// </summary>
    /// <param name="enable">If true, sets pose to ready to record state.</param>
    public override void WarnAboutRecording(bool enable)
    {
        _handAnimator?.SetBool("ReadyToRecord", enable);
    }

    /// <summary>
    /// Does nothing.
    /// </summary>
    /// <param name="enable"></param>
    public override void WarnAboutTeleport(bool enable) { }

    /// <summary>
    /// Makes hand transparent or resets it.
    /// </summary>
    /// <param name="enable">If true, makes hand transparent.</param>
    public override void WarnAboutPause(bool enable)
    {
        if (enable)
        {
            _prevState = State;
            State = VisualizerState.Invisible;
        }
        else
            State = _prevState;
    }

    /// <summary>
    /// Does nothing.
    /// </summary>
    /// <param name="enable"></param>
    public override void ShowPause(bool enable) { }

    /// <summary>
    /// Does nothing.
    /// </summary>
    /// <param name="enable"></param>
    public override void DisablePause(bool enable) { }
}
