using UnityEngine;
using DG.Tweening;

public class E_PowerFistEffect : Effect
{
    public float airborneHeight = 2f;
    public float duration = 0.4f;

    public override void Apply(Actor source, Actor target)
    {
        if (target == null)
            return;

        Debug.Log($"[E_PowerFistEffect] {target.name} 에어본!");
        Vector3 up = new Vector3(0, airborneHeight, 0);

        target.transform.DOMoveY(target.transform.position.y + airborneHeight, duration * 0.5f)
              .SetEase(Ease.OutQuad)
              .OnComplete(() =>
              {
                  target.transform.DOMoveY(target.transform.position.y, duration * 0.5f)
                        .SetEase(Ease.InQuad);
              });
    }
}
