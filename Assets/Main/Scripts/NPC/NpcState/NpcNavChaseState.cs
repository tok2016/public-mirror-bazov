using UnityEngine;

public class NpcNavChaseState : NpcNavState
{
    private float _distanceToTarget;

    public NpcNavChaseState(NpcNavigatable character) : base(character) { }

    public override void Enter(Transform target, float distanceToStop)
    {
        base.Enter(target, distanceToStop);
        _character.Agent.isStopped = false;
        _character.Agent.SetDestination(_target.position);
    }

    public override void Update()
    {
        _distanceToTarget = (_character.PlayerCamera.position - _character.transform.position).magnitude;
        if (_distanceToTarget > _character.Agent.stoppingDistance + 0.2f)
            _character.Agent.SetDestination(_target.position);
    }

    public override void Exit()
    {
        _character.Agent.isStopped = true;
    }
}
