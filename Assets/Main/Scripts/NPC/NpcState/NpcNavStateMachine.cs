using UnityEngine;

/// <summary>
/// Manages <c>NpcNavigatable</c> behaviour states.
/// </summary>
public class NpcNavStateMachine
{
    /// <value>
    /// NPC idle behaviour state. 
    /// </value>
    public NpcNavIdleState IdleState { get; private set; }

    /// <value>
    /// NPC chasing behaviour state.
    /// </value>
    public NpcNavChaseState ChasingState { get; private set; }

    /// <value>
    /// NPC coming up behaviour state.
    /// </value>
    public NpcNavComeUpState ComeUpState { get; private set; }

    /// <value>
    /// NPC looking behaviour state.
    /// </value>
    public NpcNavLookState LookState { get; private set; }

    private NpcNavState _currentState;

    /// <summary>
    /// Initializes state machine properties and states.
    /// </summary>
    /// <param name="character"></param>
    /// <param name="initialTarget"></param>
    public NpcNavStateMachine(NpcNavigatable character, Transform initialTarget)
    {
        IdleState = new NpcNavIdleState(character);
        ChasingState = new NpcNavChaseState(character);
        ComeUpState = new NpcNavComeUpState(character);
        LookState = new NpcNavLookState(character);

        _currentState = IdleState;
        _currentState.Enter(initialTarget, 0);
    }

    /// <summary>
    /// Exits currently active state, replaces it with a given one and enters the second.
    /// </summary>
    /// <param name="nextState">State to replace the current one with.</param>
    /// <param name="target">Transform to go or rotate to.</param>
    /// <param name="distanceToStop">Min distance to target to make NPC stop or change state.</param>
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
