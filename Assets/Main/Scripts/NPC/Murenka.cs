using UnityEngine;

/// <summary>
/// Represents Murenka character.
/// </summary>
public class Murenka : NpcNavigatable
{
    [Header("Murenka")]
    [SerializeField] private NpcGrabbable _objectToEat;
    [SerializeField] private GameObject _eatEffect;
    [SerializeField] private DialogueLine _purr;

    protected override void Update()
    {
        base.Update();
        MoveAnimator(Agent.velocity);
    }

    /// <summary>
    /// Immitates eating by playing particles effect.
    /// </summary>
    /// <param name="grabbable"></param>
    /// <param name="socket"></param>
    public void Eat(NpcGrabbable grabbable, NpcSocket socket) 
    {
        if(grabbable == _objectToEat)
        {
            PlayLine(_purr);
            _eatEffect.SetActive(true);
            grabbable.gameObject.SetActive(false);
        }
    }

    public override void MoveAnimator(Vector3 velocity)
    {
        _animator.SetFloat("Speed", velocity.magnitude);
    }

    public override void RotateAnimator(float rotationSpeed)
    {
        _animator.SetFloat("Speed", rotationSpeed);
    }
}
