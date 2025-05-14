using UnityEngine;
using Cysharp.Threading.Tasks;

public class Q_RoketGrabTargeting : MonoBehaviour
{
    [Header("참조 오브젝트")]
    public Camera mainCamera;
    public GameObject arrowObject; // 라인렌더러 포함
    public GameObject arrowHeadPrefab;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    [Header("설정")]
    public float arrowLength = 10f;

    private LineRenderer lineRenderer;
    private GameObject arrowHeadInstance;
    private bool isTargeting = false;

    private Q_RocketGrabSkill skill;

    private void Awake()
    {
        mainCamera = Camera.main;

        skill = new Q_RocketGrabSkill(0);

        if (arrowObject != null)
        {
            lineRenderer = arrowObject.GetComponent<LineRenderer>();
            arrowObject.SetActive(false);
        }

        if (arrowHeadPrefab != null)
        {
            arrowHeadInstance = Instantiate(arrowHeadPrefab);
            arrowHeadInstance.SetActive(false);
        }
    }

    private void Start()
    {
        MonitorQSkillLoop().Forget();
    }

    private async UniTaskVoid MonitorQSkillLoop()
    {
        while (true)
        {
            await UniTask.Yield();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                isTargeting = !isTargeting;
                arrowObject.SetActive(isTargeting);
                arrowHeadInstance?.SetActive(isTargeting);

                if (isTargeting)
                    await HandleTargetingLoop();
            }
        }
    }

    private async UniTask HandleTargetingLoop()
    {
        while (isTargeting)
        {
            await UniTask.Yield();

            UpdateArrowVisual();

            if (Input.GetMouseButtonDown(0))
            {
                FireProjectile();
                isTargeting = false;
                arrowObject?.SetActive(false);
                arrowHeadInstance?.SetActive(false);
                return;
            }
        }
    }

    private void UpdateArrowVisual()
    {
        Vector3 start = transform.position;
        start.y = 1f;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 hitPoint = hit.point;
            hitPoint.y = 1f;

            Vector3 direction = (hitPoint - start).normalized;
            Vector3 end = start + direction * arrowLength;

            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

            if (arrowHeadInstance != null)
            {
                arrowHeadInstance.transform.position = end;
                arrowHeadInstance.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }


    private void FireProjectile()
    {
        Vector3 start = projectileSpawnPoint.position;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPoint = hit.point;
            targetPoint.y = start.y;

            Vector3 dir = (targetPoint - start).normalized;

            if (!skill.CanCastExternally())
            {
                Debug.Log("Q 스킬 쿨타임 중");
                return;
            }

            // 발사체 생성
            GameObject proj = Instantiate(projectilePrefab, start, Quaternion.LookRotation(dir));
            var projScript = proj.GetComponent<Q_RoketGrabProjectile>();
            if (projScript != null)
            {
                Actor self = transform.GetComponentInParent<Actor>();
                projScript.Initialize(dir, self, skill);
            }

            // 쿨타임 설정
            skill.SetCooldownExternally(); 
        }
    }

}
