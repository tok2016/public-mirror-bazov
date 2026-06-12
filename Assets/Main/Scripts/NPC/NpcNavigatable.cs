using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NpcNavigatable : NpcContoller, INavigatable
{
    [Header("Navigation")]
    [field: SerializeField] public Transform PlayerCamera { get; private set; }
    [SerializeField] protected Transform _playerWarpPosition;
    [SerializeField] protected float _itemDistanceToStop = 0.3f;
    [SerializeField] protected float _playerDistanceToStop = 1.5f;

    public NavMeshAgent Agent { get; private set; }
    protected NpcNavStateMachine _stateMachine;

    protected override void Awake()
    {
        base.Awake();
        Agent = GetComponent<NavMeshAgent>();
        _stateMachine = new NpcNavStateMachine(this, transform);
    }

    protected override void Update()
    {
        base.Update();
        _stateMachine.Update();
    }

    public void Chase(Transform target)
    {
        Chase(target, _itemDistanceToStop);
    }

    public virtual void Chase(Transform target, float distanceToStop)
    {
        _stateMachine.ChangeState(_stateMachine.ChasingState, target, distanceToStop);
    }

    public virtual void Stop()
    {
        _stateMachine.ChangeState(_stateMachine.IdleState, transform, _itemDistanceToStop);
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
        _stateMachine.ChangeState(_stateMachine.ComeUpState, target, distanceToStop);
    }

    public void LookAtPlayer()
    {
        LookAtTarget(PlayerCamera);
    }

    public void LookAtTarget(Transform target)
    {
        _stateMachine.ChangeState(_stateMachine.LookState, target, _itemDistanceToStop);
    }

    public void RotateAnimator(float rotationDifference)
    {
        _animator.SetFloat("Rotation", rotationDifference);
    }
}
