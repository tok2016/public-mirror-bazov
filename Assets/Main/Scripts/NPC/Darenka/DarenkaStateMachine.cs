using UnityEngine;

public class DarenkaStateMachine
{
    public DarenkaIdleState IdleState { get; private set; }
    public DarenkaChasingState ChasingState { get; private set; }

    private DarenkaState _currentState;

    public DarenkaStateMachine(Darenka darenka, Transform initialTarget)
    {
        IdleState = new DarenkaIdleState(darenka);
        ChasingState = new DarenkaChasingState(darenka);

        _currentState = IdleState;
        _currentState.Enter(initialTarget, 0);
    }

    public void ChangeState(DarenkaState nextState, Transform target, float distanceToStop)
    {
        _currentState.Exit();
        _currentState = nextState;
        _currentState.Enter(target, distanceToStop);
    }

    public void Update()
    {
        _currentState.Update();
    }
}
