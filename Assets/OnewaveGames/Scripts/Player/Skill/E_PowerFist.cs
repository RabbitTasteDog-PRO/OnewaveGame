using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

// public class E_PowerFist : MonoBehaviour
// {




//     private async UniTaskVoid MonitorEKeyLoop()
//     {
//         while (true)
//         {
//             await UniTask.Yield();

//             if (Input.GetKeyDown(KeyCode.E))
//             {
//                 Debug.Log("[E_PowerFist] 조준 시작");
//                 skillCTS?.Cancel();
//                 skillCTS = new CancellationTokenSource();
//                 await WaitForClick(skillCTS.Token);
//             }
//         }
//     }

//     private async UniTask WaitForClick(CancellationToken token)
//     {
//         float elapsed = 0f;

//         if (rangeCircle != null)
//         {
//             rangeCircle.transform.localScale = new Vector3(skillRange * 2, skillRange * 2, 1);
//             rangeCircle.SetActive(true);
//         }

//         while (elapsed < maxWaitTime)
//         {
//             token.ThrowIfCancellationRequested();

//             if (Input.GetMouseButtonDown(0))
//             {
//                 rangeCircle?.SetActive(false);

//                 Collider[] hits = Physics.OverlapSphere(transform.position, skillRange);
//                 foreach (var hit in hits)
//                 {
//                     var target = hit.GetComponentInParent<Actor>();
//                     if (target != null && target != selfActor)
//                     {
//                         Debug.Log($"[E_PowerFist] {target.name}에게 스킬 발동");
//                         var skill = new E_PowerFistSkill();
//                         skill.ApplySkill(selfActor, target);
//                     }
//                 }

//                 return;
//             }

//             await UniTask.Yield();
//             elapsed += Time.deltaTime;
//         }

//         Debug.Log("[E_PowerFist] 입력 대기 시간 초과");
//         rangeCircle?.SetActive(false);
//     }

//     private void OnDestroy()
//     {
//         skillCTS?.Cancel();
//         skillCTS?.Dispose();
//     }

//     private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, skillRange);
//     }
// }

// using UnityEngine;
// using Cysharp.Threading.Tasks;

public class E_PowerFist : MonoBehaviour
{

    [Header("스킬 설정")]
    public float skillRange = 3f;
    public float maxWaitTime = 5f;

    [Header("범위")]
    public GameObject rangeCircle; // 범위 원 프리팹

    private Actor selfActor;
    private CancellationTokenSource skillCTS;

    private E_PowerFistSkill skill;
    private bool isReady = false;

    private void Awake()
    {
        if (rangeCircle != null)
        {
            rangeCircle.transform.localPosition = new Vector3(0, -0.9f, 0);
            rangeCircle.SetActive(false);
        }
        skill = new E_PowerFistSkill(0); // 초기 레벨
    }

    private void Start()
    {
        selfActor = GetComponentInParent<Actor>();
        if (selfActor == null)
        {
            Debug.LogError("[E_PowerFist] 상위에서 Actor를 찾을 수 없습니다.");
            return;
        }

        MonitorEKeyLoop().Forget();
    }

    private async UniTaskVoid MonitorEKeyLoop()
    {
        while (true)
        {
            await UniTask.Yield();

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!skill.CanCastExternally())
                {
                    Debug.Log("E 스킬 쿨타임 중입니다.");
                    continue;
                }


                Debug.Log("[E] 다음 평타에 Power Fist 적용 대기...");
                isReady = true;
                
                if (rangeCircle != null)
                {
                    rangeCircle.transform.localScale = new Vector3(skillRange * 2, skillRange * 2, 1);
                    rangeCircle.SetActive(isReady);
                }

                await WaitForLeftClick(); // 평타 대기

                isReady = false;

                Debug.Log("[E_PowerFist] 입력 대기 시간 초과");
                rangeCircle?.SetActive(isReady);
            }
        }
    }

    private async UniTask WaitForLeftClick()
    {
        while (true)
        {
            await UniTask.Yield();

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        Actor self = GetComponentInParent<Actor>();
                        Actor target = hit.collider.GetComponentInParent<Actor>();

                        skill.ApplySkill(self, target); // ✅ 에어본 적용
                        return;
                    }
                }
            }
        }
    }
}

