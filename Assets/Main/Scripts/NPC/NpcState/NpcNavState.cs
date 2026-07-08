using UnityEngine;

/// <summary>
/// Base class for all bahaviour states of NPC.
/// </summary>
public abstract class NpcNavState
{
    protected Transform _target;
    protected NpcNavigatable _character;

    /// <summary>
    /// Initializes state properties.
    /// </summary>
    /// <param name="character"></param>
    public NpcNavState(NpcNavigatable character)
    {
        _character = character;
    }

    /// <summary>
    /// Runs logic when entering this state. Stores target and distance to make NPC stop.
    /// </summary>
    /// <param name="target">Transform to go and rotate to.</param>
    /// <param name="distanceToStop">Min distance to make NPC stop.</param>
    public virtual void Enter(Transform target, float distanceToStop)
    {
        _target = target;
        _character.Agent.stoppingDistance = distanceToStop;
    }

    /// <summary>
    /// Runs logic of state every frame.
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// Runs logic when exiting this state.
    /// </summary>
    public virtual void Exit() { }
}
