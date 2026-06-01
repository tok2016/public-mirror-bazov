using UnityEngine;

public abstract class DarenkaState
{
    protected Darenka _darenka;
    protected Transform _target;

    public DarenkaState(Darenka darenka) 
    {
        _darenka = darenka;
    }

    public virtual void Enter(Transform target, float distanceToStop) 
    {
        _target = target;
        _darenka.Agent.stoppingDistance = distanceToStop;
    }

    public virtual void Update() { }

    public virtual void Exit() { }
}
