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
        if((_darenka.Camera.position - _darenka.transform.position).magnitude > _darenka.Agent.stoppingDistance + 0.2f)
            _darenka.Agent.SetDestination(_target.position);
    }

    public override void Exit()
    {
        _darenka.Agent.isStopped = true;
    }
}
