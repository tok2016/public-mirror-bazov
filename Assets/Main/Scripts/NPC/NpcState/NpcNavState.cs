using UnityEngine;

public class NpcNavState
{
    protected Transform _target;
    protected NpcNavigatable _character;

    public NpcNavState(NpcNavigatable character)
    {
        _character = character;
    }

    public virtual void Enter(Transform target, float distanceToStop)
    {
        _target = target;
        _character.Agent.stoppingDistance = distanceToStop;
    }

    public virtual void Update() { }

    public virtual void Exit() { }
}
