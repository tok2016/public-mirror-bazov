using UnityEngine;

public class NpcNavStateMachine
{
    public NpcNavIdleState IdleState { get; private set; }
    public NpcNavChaseState ChasingState { get; private set; }
    public NpcNavComeUpState ComeUpState { get; private set; }
    public NpcNavLookState LookState { get; private set; }

    private NpcNavState _currentState;

    public NpcNavStateMachine(NpcNavigatable character, Transform initialTarget)
    {
        IdleState = new NpcNavIdleState(character);
        ChasingState = new NpcNavChaseState(character);
        ComeUpState = new NpcNavComeUpState(character);
        LookState = new NpcNavLookState(character);

        _currentState = IdleState;
        _currentState.Enter(initialTarget, 0);
    }

    public void ChangeState(NpcNavState nextState, Transform target, float distanceToStop)
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
