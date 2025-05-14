using UnityEngine;
using Cysharp.Threading.Tasks;

public class W_OverdriveEffect : Effect
{
    public float speedMultiplier = 1.5f;
    public float duration = 3f;

    public override void Apply(Actor source, Actor target)
    {
        if (source == null)
        {
            return;
        }

        var actor = source;

        float originalSpeed = actor.moveSpeed;
        actor.GetComponent<MoveController>().moveSpeed = originalSpeed * speedMultiplier;
        Debug.Log($"[W_OverdriveEffect] 이동속도 {originalSpeed} → {actor.moveSpeed} (x{speedMultiplier})");

        RestoreAfterDelay(actor, originalSpeed).Forget();
    }

    private async UniTaskVoid RestoreAfterDelay(Actor actor, float originalSpeed)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(duration));
        actor.GetComponent<MoveController>().moveSpeed = originalSpeed;

        Debug.Log($"[W_OverdriveEffect] 이동속도 원래대로 복구: {originalSpeed}");
    }
}
