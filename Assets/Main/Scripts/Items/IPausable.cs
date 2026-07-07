/// <summary>
/// Declares behaviour for object able to freeze while pause.
/// </summary>
public interface IPausable
{
    /// <summary>
    /// Freezes object behaviour. 
    /// </summary>
    public void Freeze();

    /// <summary>
    /// Continues object behaviour.
    /// </summary>
    public void Unfreeze();
}
