using UnityEngine;

public class Kokovanya : NpcNavigatable
{
    protected override void Update()
    {
        base.Update();
        _animator.SetFloat("Speed", Agent.velocity.magnitude);
    }
}
