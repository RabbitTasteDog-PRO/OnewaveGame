using UnityEngine;
using DG.Tweening;

public class PullEffect : Effect
{
    public float pullDuration = 0.3f;
    public float pullDistance = 1.5f;


    public override void Apply(Actor source, Actor target)
    {
        if (source == null || target == null)
        {
            Debug.LogWarning("[PullEffect] source 또는 target이 null입니다.");
            return;
        }

        Vector3 destination = source.transform.position + source.transform.forward * 1.5f;

        Debug.Log($"[PullEffect] {target.name} 끌기 , {destination}");

        target.PullTo(destination); 
    }
}


