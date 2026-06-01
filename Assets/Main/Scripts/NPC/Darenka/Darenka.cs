using UnityEngine;
using UnityEngine.AI;

public class Darenka : MonoBehaviour
{
    [field: SerializeField] public Transform Camera {  get; private set; }
    [SerializeField] private Transform _warpPosition;
    [SerializeField] private float _itemDistanceToStop;
    [SerializeField] private float _playerDistanceToStop;
    [SerializeField] private Animator _animator;

    public NavMeshAgent Agent { get; private set; }
    private DarenkaStateMachine _stateMachine;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        _stateMachine = new DarenkaStateMachine(this, transform);
    }

    void Update()
    {
        _stateMachine.Update();
        _animator.SetFloat("Speed", Agent.velocity.magnitude);
    }

    public void Chase(Transform target)
    {
        _stateMachine.ChangeState(_stateMachine.ChasingState, target, _itemDistanceToStop);
    }

    public void Chase(Transform target, float distanceToStop)
    {
        _stateMachine.ChangeState(_stateMachine.ChasingState, target, distanceToStop);
    }

    public void Stop()
    {
        _stateMachine.ChangeState(_stateMachine.IdleState, transform, _itemDistanceToStop);
    }

    public void GoToPlayer()
    {
        Chase(Camera, _playerDistanceToStop);
    }

    public void Warp()
    {
        Agent.Warp(_warpPosition.position);
        transform.LookAt(Camera);
    }
}
