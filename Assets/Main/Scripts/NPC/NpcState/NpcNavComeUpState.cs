using UnityEngine;

public class NpcNavComeUpState : NpcNavState
{
    public NpcNavComeUpState(NpcNavigatable character) : base(character) { }

    public override void Enter(Transform target, float distanceToStop)
    {
        base.Enter(target, distanceToStop);
        _character.Agent.isStopped = false;
        _character.Agent.SetDestination(_target.position);
    }

    public override void Exit()
    {
        _character.Agent.isStopped = true;
    }
}
