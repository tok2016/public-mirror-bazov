
public class Darenka : NpcNavigatable
{
    protected override void Update()
    {
        base.Update();
        _animator.SetFloat("Speed", Agent.velocity.magnitude);
    }
}
