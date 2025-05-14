using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Actor : MonoBehaviour
{
    [Header("Pull 설정")]
    public float pullDuration = 0.3f;
    public Ease pullEase = Ease.InSine;

    private bool isPulled = false;
    public float moveSpeed = 5f; // 기본 이동속도

    public void PullTo(Vector3 targetPosition)
    {
        Debug.Log($"[Actor] {name} , PullTo : {targetPosition}");

        transform.DOMove(targetPosition, pullDuration)
                 .SetEase(pullEase)
                 .OnComplete(() =>
                 {
                     Debug.Log($"Q Skill End");
                 });
    }

    // public void PullTo(Vector3 targetPosition)
    // {
    //     targetPosition.y = transform.position.y; // 내 높이 유지

    //     transform.DOMove(targetPosition, pullDuration)
    //              .SetEase(pullEase)
    //              .OnComplete(() =>
    //              {
    //                  Debug.Log($"[Actor] {name} 도착 완료");
    //              });
    // }


    /// <summary>
    /// 외부에서 스킬 대상이 되었을 때 호출됨 (선택적 후처리)
    /// </summary>
    public void ApplySkill(Actor source)
    {
        Debug.Log($"[Actor] {name}, {source.name}의 스킬에 피격당함");
    }
}

