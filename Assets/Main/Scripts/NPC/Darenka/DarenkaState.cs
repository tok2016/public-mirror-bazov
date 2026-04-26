using UnityEngine;

public abstract class DarenkaState
{
    protected Darenka _darenka;
    protected Transform _target;
    protected float _distanceToStop;

    public DarenkaState(Darenka darenka) 
    {
        _darenka = darenka;
    }

    public virtual void Enter(Transform target, float distanceToStop) 
    {
        _target = target;
        _distanceToStop = distanceToStop;
    }

    public virtual void Update() { }

    public virtual void Exit() { }
}
