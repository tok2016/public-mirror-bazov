using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class NpcNavigatable : NpcContoller, INavigatable
{
    [Header("Navigation")]
    [field: SerializeField] public Transform PlayerCamera { get; private set; }
    [SerializeField] protected Transform _playerWarpPosition;
    [SerializeField] protected float _itemDistanceToStop = 0.3f;
    [SerializeField] protected float _playerDistanceToStop = 1.5f;

    public UnityEvent startAction;

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

    private void ChangeState(NpcNavState state, Transform target, float distanceToStop)
    {
        _savedStopDistance = distanceToStop;
        _savedTarget = target;
        _savedState = state;
        _stateMachine.ChangeState(state, target, distanceToStop);
    }

    public void Chase(Transform target)
    {
        Chase(target, _itemDistanceToStop);
    }

    public virtual void Chase(Transform target, float distanceToStop)
    {
        ChangeState(_stateMachine.ChasingState, target, distanceToStop);
    }

    public virtual void Stop()
    {
        ChangeState(_stateMachine.IdleState, transform, _itemDistanceToStop);
    }

    public void ChaseAfterPlayer()
    {
        Chase(PlayerCamera, _playerDistanceToStop);
    }

    public virtual void Warp(Transform target)
    {
        Agent.Warp(target.position);
        transform.LookAt(target);
    }

    public void WarpToPlayer()
    {
        Agent.Warp(_playerWarpPosition.position);
        transform.LookAt(PlayerCamera);
    }

    public void ComeUp(Transform target)
    {
        ComeUp(target, _itemDistanceToStop);
    }

    public virtual void ComeUp(Transform target, float distanceToStop)
    {
        ChangeState(_stateMachine.ComeUpState, target, distanceToStop);
    }

    public void LookAtPlayer()
    {
        LookAtTarget(PlayerCamera);
    }

    public void LookAtTarget(Transform target)
    {
        ChangeState(_stateMachine.LookState, target, _itemDistanceToStop);
    }

    public abstract void MoveAnimator(Vector3 velocity);
    public abstract void RotateAnimator(float rotationSpeed);

    public override void Freeze()
    {
        base.Freeze();
        _stateMachine.ChangeState(_stateMachine.IdleState, transform, _itemDistanceToStop);
    }

    public override void Unfreeze()
    {
        base.Unfreeze();
        _stateMachine.ChangeState(_savedState, _savedTarget, _savedStopDistance);
    }
}
