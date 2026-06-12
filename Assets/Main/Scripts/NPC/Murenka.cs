using UnityEngine;

public class Murenka : NpcNavigatable
{
    [Header("Murenka")]
    [SerializeField] private NpcGrabbable _objectToEat;
    [SerializeField] private GameObject _eatEffect;
    [SerializeField] private DialogueLine _purr;

    protected override void Update()
    {
        base.Update();
        _animator.SetFloat("Speed", Agent.velocity.magnitude);
    }

    public void Eat(NpcGrabbable grabbable, NpcSocket socket) 
    {
        if(grabbable == _objectToEat)
        {
            PlayLine(_purr);
            _eatEffect.SetActive(true);
            grabbable.gameObject.SetActive(false);
        }
    }
}
