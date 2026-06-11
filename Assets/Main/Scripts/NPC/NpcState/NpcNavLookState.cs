using UnityEngine;

public class NpcNavLookState : NpcNavState
{
    public NpcNavLookState(NpcNavigatable character) : base(character) { }

    public override void Update()
    {
        var prevQuaternion = _character.transform.rotation;
        _character.transform.LookAt(_target);
        _character.transform.rotation = Quaternion.Euler(0, _character.transform.rotation.eulerAngles.y, 0);
        _character.RotateAnimator((prevQuaternion.eulerAngles - _character.transform.rotation.eulerAngles).magnitude);
    }
}
