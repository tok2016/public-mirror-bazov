using UnityEngine;

public class Kokovanya : NpcNavigatable
{
    protected override void Update()
    {
        base.Update();
        MoveAnimator(Agent.velocity);
    }

    public override void MoveAnimator(Vector3 speed)
    {
        _animator.SetFloat("Speed", speed.magnitude);
    }

    public override void RotateAnimator(float rotationDifference)
    {
        _animator.SetFloat("Speed", rotationDifference);
    }
}
