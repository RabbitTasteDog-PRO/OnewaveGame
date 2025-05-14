using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*******************************************************************************/
// 상수들 정의
/*******************************************************************************/
public class Constants
{
    /// 기본 타임 스케일 
    public const float TIME_SCALE = 1.0f;
    /// 기본 deltaTime 
    // public const float FIXED_DELTATIME = 0.02f;

    public static readonly Vector3 _pParts = Vector3.zero;
    public static readonly Quaternion _qParts = new Quaternion(0, 0, 0, 0);

    public static T ConvertEnumData<T>(string _value)
    {
        return (T)System.Enum.Parse(typeof(T), _value);
    }

}
