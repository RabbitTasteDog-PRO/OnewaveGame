using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public abstract class Skill
{
    // 상속받기위헤 접근제한자 수정
    protected List<Effect> EffectList { get; } = new();
    private float lastCastTime = -999f; // 공통 쿨다운 추적
    public abstract bool ApplySkill(Actor source, Actor target);

    /******************************************************************************/
    // 쿨타임 추상화 
    /// <summary>
    /// 쿨타임 체크 후 true면 사용 가능
    /// </summary>
    protected bool IsCooldownReady(float cooldownSeconds)
    {
        float elapsed = Time.time - lastCastTime;
        return elapsed >= cooldownSeconds;
    }

    protected float RemainingCooldown(float cooldownSeconds)
    {
        float elapsed = Time.time - lastCastTime;
        return Mathf.Max(0f, cooldownSeconds - elapsed);
    }

    protected void SetCooldown()
    {
        lastCastTime = Time.time;
        Debug.Log($"#### lastCastTime : {lastCastTime}");
    }

    /// <summary>
    /// 외부에서 type과 level을 넘겨서 쿨타임 가능 여부 판단
    /// </summary>
    public bool CanCast(eSkillType type, int level)
    {
        if (!SkillManager.Instance.TryGetSkillData(type, level, out var data))
            return false;

        float cooldownSec = data.coolTime / 1000f;
        return IsCooldownReady(cooldownSec);
    }

    public void SetCooldownIfValid(eSkillType type, int level)
    {
        if (SkillManager.Instance.TryGetSkillData(type, level, out var data))
            SetCooldown();
    }

}
