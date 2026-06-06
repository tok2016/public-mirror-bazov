using UnityEngine;

public interface INavigatable
{
    public void Chase(Transform target);

    public void Chase(Transform target, float distanceToStop);

    public void Stop();

    public void ChaseAfterPlayer();

    public void Warp(Transform target);

    public void WarpToPlayer();

    public void ComeUp(Transform target);

    public void ComeUp(Transform target, float distanceToStop);
}
