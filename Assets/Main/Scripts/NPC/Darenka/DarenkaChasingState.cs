using UnityEngine;

public class DarenkaChasingState : DarenkaState
{
    public DarenkaChasingState(Darenka darenka) : base(darenka) { }

    public override void Enter(Transform target, float distanceToStop)
    {
        base.Enter(target, distanceToStop);
        _darenka.Agent.isStopped = false;
        _darenka.Agent.SetDestination(_target.position);
    }

    public override void Update()
    {
        _darenka.Agent.SetDestination(_target.position);
    }

    public override void Exit()
    {
        _darenka.Agent.isStopped = true;
    }
}
