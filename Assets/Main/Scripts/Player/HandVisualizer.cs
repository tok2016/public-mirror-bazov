using UnityEngine;

public class HandVisualizer : ActionsVisualizer
{
    private Animator _handAnimator;

    private void Awake()
    {
        _handAnimator = GetComponent<Animator>();
    }

    public override void DisableGrab(bool enable)
    {
        if(enable)
        {
            _handAnimator?.SetBool("ReadyToGrab", false);
            _handAnimator?.SetBool("IsGrabbing", false);
        }
    }

    public override void DisablePoke(bool enable)
    {
        if (enable)
            _handAnimator?.SetBool("IsPoking", false);
    }

    public override void DisableRecording(bool enable)
    {
        if (enable)
        {
            _handAnimator?.SetBool("ReadyToRecord", false);
            _handAnimator?.SetBool("IsRecording", false);
        }
    }

    public override void DisableTeleport(bool enable)
    {
        DisablePoke(enable);
    }

    public override void ShowGrab(bool enable)
    {
        _handAnimator?.SetBool("IsGrabbing", enable);
    }

    public override void ShowPoke(bool enable)
    {
        if (enable)
            _handAnimator?.SetTrigger("Poke");
    }

    public override void ShowRecording(bool enable)
    {
        _handAnimator?.SetBool("IsRecording", enable);
    }

    public override void ShowTeleport(bool enable)
    {
        WarnAboutPoke(enable);
    }

    public override void WarnAboutGrab(bool enable)
    {
        _handAnimator?.SetBool("ReadyToGrab", enable);
    }

    public override void WarnAboutPoke(bool enable)
    {
        _handAnimator?.SetBool("IsPoking", enable);
    }

    public override void WarnAboutRecording(bool enable)
    {
        _handAnimator?.SetBool("ReadyToRecord", enable);
    }

    public override void WarnAboutTeleport(bool enable) { }
}
