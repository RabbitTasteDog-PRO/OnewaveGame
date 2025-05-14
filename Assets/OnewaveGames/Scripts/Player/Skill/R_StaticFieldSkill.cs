using UnityEngine;
using Enums;

public class R_StaticFieldSkill : Skill
{
    private int level;
    private eSkillType skillType = eSkillType.StaticField;

    public R_StaticFieldSkill(int level = 0)
    {
        this.level = Mathf.Clamp(level, 0, 9);
        EffectList.Add(new R_StaticFieldEffect());
    }

    public override bool ApplySkill(Actor source, Actor target)
    {
        if (source == null)
            return false;

        if (!SkillManager.Instance.TryGetSkillData(skillType, level, out var data))
        {
            Debug.LogWarning("[R] SkillData를 찾지 못함");
            return false;
        }

        float cooldownSec = data.coolTime * 0.001f;

        if (!IsCooldownReady(cooldownSec))
        {
            Debug.Log($"[R] 쿨타임 중... {RemainingCooldown(cooldownSec):F2}s 남음");
            return false;
        }

        SetCooldown();

        foreach (var effect in EffectList)
        {
            effect.Apply(source, null); // 범위 효과, 타겟 없음
        }

        Debug.Log($"[R] Static Field 발동! (Lv{level + 1})");
        return true;
    }

    public bool CanCastExternally() => CanCast(skillType, level);
    public void SetCooldownExternally() => SetCooldownIfValid(skillType, level);
}
