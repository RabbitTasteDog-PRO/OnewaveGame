using UnityEngine;
using Enums;
using System;
using System.Collections;


public class Q_RocketGrabSkill : Skill
{
    private int level = 0;
    float cooldownSec ;
    private eSkillType skillType = eSkillType.RocketGrab;

    public Q_RocketGrabSkill(int level = 0)
    {
        this.level = Mathf.Clamp(level, 0, 9);
        EffectList.Add(new PullEffect());
    }

    public override bool ApplySkill(Actor source, Actor target)
    {
        if (source == null || target == null)
            return false;

        if (!IsCooldownReady(cooldownSec))
        {
            Debug.Log($"[Q] 쿨타임 중: {RemainingCooldown(cooldownSec):F2}초 남음");
            return false;
        }

        if (!SkillManager.Instance.TryGetSkillData(skillType, level, out var skillData))
            return false;

        cooldownSec = skillData.coolTime * 0.001f;

        SetCooldown();
        Debug.Log($"[Q_RocketGrabSkill] 레벨: Lv{level + 1}, 쿨타임: {skillData.coolTime}ms");
        Debug.Log($"[Q] 스킬 사용됨 (Lv{level + 1}) 쿨타임: {cooldownSec}s");
        
        foreach (var effect in EffectList)
        {
            effect.Apply(source, target);
        }

        Debug.Log($"[Q] {source.name} / {target.name} 적중 (Lv{level + 1})");
        return true;
    }

    public bool CanCastExternally() => CanCast(skillType, level);
    public void SetCooldownExternally() => SetCooldownIfValid(skillType, level);

}



