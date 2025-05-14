using UnityEngine;
using Cysharp.Threading.Tasks;

public class W_Overdrive : MonoBehaviour
{
    private W_OverdriveSkill skill;

    void Awake()
    {
        skill = new W_OverdriveSkill(0); // 레벨 필요 시 외부 주입
    }

    void Start()
    {
        MonitorWKeyLoop().Forget();
    }

    private async UniTaskVoid MonitorWKeyLoop()
    {
        while (true)
        {
            await UniTask.Yield();

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (!skill.CanCastExternally())
                {
                    Debug.Log("W 스킬 쿨타임 중입니다.");
                    continue;
                }

                Actor self = GetComponentInParent<Actor>();
                skill.ApplySkill(self, null); // W는 타겟 없음
                // skill.SetCooldownExternally(); ← ApplySkill에서 이미 호출되므로 불필요
            }
        }
    }
}
