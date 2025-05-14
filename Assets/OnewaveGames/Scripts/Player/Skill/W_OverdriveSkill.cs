using UnityEngine;
using Enums;

public class W_OverdriveSkill : Skill
{
    private int level;
    private eSkillType skillType = eSkillType.OverDrive;

    public W_OverdriveSkill(int level = 0)
    {
        this.level = Mathf.Clamp(level, 0, 9);
        EffectList.Add(new W_OverdriveEffect());
    }

    public override bool ApplySkill(Actor source, Actor target)
    {
        if (source == null)
            return false;

        if (!SkillManager.Instance.TryGetSkillData(skillType, level, out var data))
        {
            Debug.LogWarning("[W] SkillData를 찾지 못함");
            return false;
        }

        float cooldownSec = data.coolTime / 1000f;

        if (!IsCooldownReady(cooldownSec))
        {
            Debug.Log($"[W] 쿨타임 중... {RemainingCooldown(cooldownSec):F2}s 남음");
            return false;
        }

        SetCooldown(); // ✅ 쿨 시작

        foreach (var effect in EffectList)
            effect.Apply(source, null); // W는 타겟 없음

        Debug.Log($"[W] Overdrive 스킬 발동 (Lv{level + 1}), 쿨타임: {cooldownSec}s");
        return true;
    }

    public bool CanCastExternally() => CanCast(skillType, level);
    public void SetCooldownExternally() => SetCooldownIfValid(skillType, level);
}
