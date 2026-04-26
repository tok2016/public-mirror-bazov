using UnityEngine;
using UnityEngine.AI;

public class Darenka : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _itemDistanceToStop;
    [SerializeField] private float _playerDistanceToStop;

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
        Chase(_player, _playerDistanceToStop);
    }
}
