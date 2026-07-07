using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// Base class for all NPC who are able to navigate and move.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public abstract class NpcNavigatable : NpcContoller, INavigatable
{
    /// <value>
    /// Player transform to follow.
    /// </value>
    [Header("Navigation")]
    [field: SerializeField] public Transform PlayerCamera { get; private set; }
    [SerializeField] protected Transform _playerWarpPosition;
    [SerializeField] protected float _itemDistanceToStop = 0.3f;
    [SerializeField] protected float _playerDistanceToStop = 1.5f;

    public UnityEvent startAction;

    /// <value>
    /// Component that lets NPC navigate in the scene.
    /// </value>
    public NavMeshAgent Agent { get; private set; }
    protected NpcNavStateMachine _stateMachine;

    private Transform _savedTarget;
    private float _savedStopDistance;
    private NpcNavState _savedState;

    protected override void Awake()
    {
        base.Awake();
        Agent = GetComponent<NavMeshAgent>();
        _stateMachine = new NpcNavStateMachine(this, transform);
        _savedState = _stateMachine.IdleState;
        _savedTarget = transform;
    }

    protected override void Start()
    {
        base.Start();
        startAction?.Invoke();
    }

    protected override void Update()
    {
        base.Update();
        _stateMachine.Update();
    }

    /// <summary>
    /// Changes NPC behaviour state and stores given properties.
    /// </summary>
    /// <param name="state">State to replace with.</param>
    /// <param name="target">Tranform to follow.</param>
    /// <param name="distanceToStop">Min distance to target to make NPC stop.</param>
    private void ChangeState(NpcNavState state, Transform target, float distanceToStop)
    {
        _savedStopDistance = distanceToStop;
        _savedTarget = target;
        _savedState = state;
        _stateMachine.ChangeState(state, target, distanceToStop);
    }

    /// <summary>
    /// Makes NPC chase after given target and stops them at default distance.
    /// </summary>
    /// <remarks>
    /// Changes NPC position and rotation.
    /// </remarks>
    /// <param name="target">Transform to chase after.</param>
    public void Chase(Transform target)
    {
        Chase(target, _itemDistanceToStop);
    }

    /// <summary>
    /// Makes NPC chase after given target and stops them at given distance.
    /// </summary>
    /// <remarks>
    /// Changes NPC position and rotation.
    /// </remarks>
    /// <param name="target">Transform to chase after.</param>
    /// <param name="distanceToStop">Min distance to target to make NPC stop.</param>
    public virtual void Chase(Transform target, float distanceToStop)
    {
        ChangeState(_stateMachine.ChasingState, target, distanceToStop);
    }

    /// <summary>
    /// Stops NPC movement and rotation.
    /// </summary>
    public virtual void Stop()
    {
        ChangeState(_stateMachine.IdleState, transform, _itemDistanceToStop);
    }

    /// <summary>
    /// Makes NPC chase after player.
    /// </summary>
    /// <remarks>
    /// Changes NPC position and rotation.
    /// </remarks>
    public void ChaseAfterPlayer()
    {
        Chase(PlayerCamera, _playerDistanceToStop);
    }

    /// <summary>
    /// Instantly places NPC at given target.
    /// </summary>
    /// <param name="target">Transform to warp at.</param>
    public virtual void Warp(Transform target)
    {
        Agent.Warp(target.position);
        transform.LookAt(target);
    }

    /// <summary>
    /// Instantly places NPC near the player.
    /// </summary>
    public void WarpToPlayer()
    {
        Agent.Warp(_playerWarpPosition.position);
        transform.LookAt(PlayerCamera);
    }

    /// <summary>
    /// Makes NPC come up to given target without updating navigation and stops at default distance.
    /// </summary>
    /// <remarks>
    /// Changes NPC position and rotation.
    /// </remarks>
    /// <param name="target">Transform to come up to.</param>
    public void ComeUp(Transform target)
    {
        ComeUp(target, _itemDistanceToStop);
    }

    /// <summary>
    /// Makes NPC come up to given target without updating navigation and stops at given distance.
    /// </summary>
    /// <remarks>
    /// Changes NPC position and rotation.
    /// </remarks>
    /// <param name="target">Transform to come up to.</param>
    /// <param name="distanceToStop">Min distance to target to make NPC stop.</param>
    public virtual void ComeUp(Transform target, float distanceToStop)
    {
        ChangeState(_stateMachine.ComeUpState, target, distanceToStop);
    }

    /// <summary>
    /// Rotates NPC towards player.
    /// </summary>
    /// <remarks>
    /// Changes only NPC rotation.
    /// </remarks>
    public void LookAtPlayer()
    {
        LookAtTarget(PlayerCamera);
    }

    /// <summary>
    /// Rotates NPC towards given target.
    /// </summary>
    /// <remarks>
    /// Changes only NPC rotation.
    /// </remarks>
    /// <param name="target">Transform to look at.</param>
    public void LookAtTarget(Transform target)
    {
        ChangeState(_stateMachine.LookState, target, _itemDistanceToStop);
    }

    /// <summary>
    /// Enable movement animation.
    /// </summary>
    /// <param name="velocity">NPC movement velocity.</param>
    public abstract void MoveAnimator(Vector3 velocity);

    /// <summary>
    /// Enables rotation animation.
    /// </summary>
    /// <param name="rotationSpeed">NPC absolute angular speed</param>
    public abstract void RotateAnimator(float rotationSpeed);

    /// <summary>
    /// Stops NPC movement and rotation on pause.
    /// </summary>
    public override void Freeze()
    {
        base.Freeze();
        _stateMachine.ChangeState(_stateMachine.IdleState, transform, _itemDistanceToStop);
    }

    /// <summary>
    /// Returns NPC to state stored before pause.
    /// </summary>
    public override void Unfreeze()
    {
        base.Unfreeze();
        _stateMachine.ChangeState(_savedState, _savedTarget, _savedStopDistance);
    }
}
