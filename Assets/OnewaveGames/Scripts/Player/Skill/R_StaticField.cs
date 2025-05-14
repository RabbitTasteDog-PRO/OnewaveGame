using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class R_StaticField : MonoBehaviour
{
    [Header("스킬 설정")]
    public float skillRange = 5f;
    public float shakeDuration = 0.5f;
    public float shakeStrength = 0.2f;
    public float cooldown = 8f;

    [Header("범위")]
    public GameObject rangeCircle; // 바닥 서클 이미지 (Quad + 투명 PNG)

    private bool isOnCooldown = false;

    void Awake()
    {
        // 처음에는 비활성화
        if (rangeCircle != null)
        {
            rangeCircle.SetActive(false);
            rangeCircle.transform.localPosition = new Vector3(0, -0.9f, 0);
        }
    }

    void Start()
    {
        MonitorRKeyLoop().Forget();
    }

    private async UniTaskVoid MonitorRKeyLoop()
    {
        while (true)
        {
            await UniTask.Yield();

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (isOnCooldown == true)
                {
                    Debug.Log("[R] Static Field 쿨타임 중 ");
                }
                else
                {
                    ActivateR().Forget();
                }
            }
        }
    }

    private async UniTaskVoid ActivateR()
    {
        Debug.Log("[R] Static Field 발동!");
        isOnCooldown = true;

        // 범위 서클 표시
        if (rangeCircle != null)
        {
            rangeCircle.transform.localScale = Vector3.one * skillRange * 2f;
            rangeCircle.SetActive(true);
        }

        // 범위 내 적 처리
        Collider[] colliders = Physics.OverlapSphere(transform.position, skillRange);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                col.transform.DOShakePosition(shakeDuration, shakeStrength, vibrato: 20, randomness: 90)
                    .SetEase(Ease.OutExpo);
            }
        }

        // 잠시 보여준 후 제거
        await UniTask.Delay(500);
        if (rangeCircle != null)
            rangeCircle.SetActive(false);

        // 쿨다운 시작
        await UniTask.Delay((int)(cooldown * 1000f));
        isOnCooldown = false;
        Debug.Log("[R] 쿨타임 완료");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, skillRange);
    }
}
