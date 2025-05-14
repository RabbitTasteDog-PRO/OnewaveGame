// using UnityEngine;
// using DG.Tweening;

// public class R_StaticFieldEffect : Effect
// {
//     public float radius = 5f;
//     public float airborneHeight = 1f;
//     public float duration = 0.3f;

//     [Header("스킬 설정")]
//     public float skillRange = 5f;
//     public float shakeDuration = 0.5f;
//     public float shakeStrength = 0.2f;
//     public float cooldown = 8f;

//     public override void Apply(Actor source, Actor target)
//     {
//         if (source == null) return;

//         Collider[] hits = Physics.OverlapSphere(source.transform.position, radius);
//         foreach (var hit in hits)
//         {
//             var enemy = hit.GetComponentInParent<Actor>();
//             if (enemy != null && enemy != source)
//             {
//                 Vector3 originalPos = enemy.transform.position;
//                 Vector3 up = originalPos + Vector3.up * airborneHeight;

//                 enemy.transform.DOShakePosition(shakeDuration, shakeStrength, vibrato: 20, randomness: 90)
//                     .SetEase(Ease.OutExpo);

//                 // 에어본 적용
//                 // enemy.transform.DOMoveY(up.y, duration * 0.5f)
//                 //      .SetEase(Ease.OutQuad)
//                 //      .OnComplete(() =>
//                 //      {
//                 //          enemy.transform.DOMoveY(originalPos.y, duration * 0.5f).SetEase(Ease.InQuad);
//                 //      });

//                 Debug.Log($"[R_StaticFieldEffect] {enemy.name}에게 궁 피해 적용");
//             }
//         }
//     }
// }


using UnityEngine;
using DG.Tweening;

public class R_StaticFieldEffect : Effect
{
    public float range = 5f;
    public float shakeDuration = 0.5f;
    public float shakeStrength = 0.2f;

    public override void Apply(Actor source, Actor target)
    {
        Debug.Log("[R_StaticFieldEffect] 범위 내 적 타격 시도");

        Collider[] enemies = Physics.OverlapSphere(source.transform.position, range);
        foreach (var col in enemies)
        {
            if (col.CompareTag("Enemy"))
            {
                var enemyActor = col.GetComponentInParent<Actor>();
                if (enemyActor != null)
                {
                    Debug.Log($"[R] {enemyActor.name} 타격");
                    col.transform.DOShakePosition(shakeDuration, shakeStrength, 20, 90)
                        .SetEase(Ease.OutExpo);
                }
            }
        }
    }
}
