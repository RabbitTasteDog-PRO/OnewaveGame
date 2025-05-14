using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class Q_RoketGrabProjectile : MonoBehaviour
{
    [Header("발사체 설정")]
    public float speed = 20f;
    public float maxDistance = 15f;

    private Vector3 direction;
    private Vector3 startPos;

    private Actor caster; // 시전자
    private Skill skill;  // 어떤 스킬인지
    private CancellationTokenSource cts;

    public void Initialize(Vector3 dir, Actor source, Skill appliedSkill)
    {
        direction = dir;
        startPos = transform.position;
        caster = source;
        skill = appliedSkill;

        cts = new CancellationTokenSource();
        MoveLoop(cts.Token).Forget();

        Debug.Log("####################### Initialize");
    }

    private async UniTaskVoid MoveLoop(CancellationToken token)
    {
        try
        {
            Debug.Log("####################### MoveLoop");
            while (Vector3.Distance(startPos, transform.position) < maxDistance)
            {
                transform.position += direction * speed * Time.deltaTime;
                token.ThrowIfCancellationRequested();
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            Destroy(gameObject);
        }
        catch (Exception e)
        {
            // 발사체 삭제된 후 동작 예외처리
            // Debug.Log($"Exceiption : {e.ToString()}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("##### OnTriggerEnter");
        // var target = other.transform.parent.transform.GetComponent<Actor>();
        // var target = other.GetComponentInParent<Actor>();
        if (other.CompareTag("Enemy"))
        {
            var target = other.transform.parent.transform.GetComponent<Actor>();
            Debug.Log($"[Q_Projectile] {caster.name} , {target.name} : 스킬 발동");
            skill.ApplySkill(caster, target);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts?.Dispose();
    }
}
