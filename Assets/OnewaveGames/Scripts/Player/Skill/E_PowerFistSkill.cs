using UnityEngine;
using Enums;

public class E_PowerFistSkill : Skill
{
    private int level;
    private eSkillType skillType = eSkillType.PowerFist;

    public E_PowerFistSkill(int level = 0)
    {
        this.level = Mathf.Clamp(level, 0, 9);
        EffectList.Add(new E_PowerFistEffect());
    }

    public override bool ApplySkill(Actor source, Actor target)
    {
        if (source == null || target == null)
            return false;

        if (!SkillManager.Instance.TryGetSkillData(skillType, level, out var data))
        {
            Debug.LogWarning("[E] SkillData를 찾지 못함");
            return false;
        }

        float cooldownSec = data.coolTime * 0.001f;

        if (!IsCooldownReady(cooldownSec))
        {
            Debug.Log($"[E] 쿨타임 중... {RemainingCooldown(cooldownSec):F2}s 남음");
            return false;
        }

        SetCooldown();

        foreach (var effect in EffectList)
        {
            effect.Apply(source, target);
        }

        Debug.Log($"[E] PowerFist 발동! (Lv{level + 1})");
        return true;
    }

    public bool CanCastExternally() => CanCast(skillType, level);
    public void SetCooldownExternally() => SetCooldownIfValid(skillType, level);
}
