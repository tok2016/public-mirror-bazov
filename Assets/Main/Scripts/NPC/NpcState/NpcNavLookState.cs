using UnityEngine;

public class NpcNavLookState : NpcNavState
{
    public NpcNavLookState(NpcNavigatable character) : base(character) { }
    private float _rotationSpeed = 3f;

    public override void Update()
    {
        var direction = new Vector3(_target.position.x, 0, _target.position.z) 
            - new Vector3(_character.transform.position.x, 0, _character.transform.position.z);
        var dot = Vector3.Dot(_character.transform.forward, direction.normalized);

        if (dot <= 0.97f)
        {
            _character.RotateAnimator(1);
            var targetRotation = Quaternion.Euler(0, Quaternion.LookRotation(direction).eulerAngles.y, 0);
            _character.transform.rotation = Quaternion.Slerp(
                _character.transform.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime
            );
        }
        else
            _character.RotateAnimator(0);
    }

    public override void Exit()
    {
        _character.RotateAnimator(0);
    }
}
