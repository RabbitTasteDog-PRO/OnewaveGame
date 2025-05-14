using System.Collections.Generic;
using UnityEngine;
using UGS;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GoogleSheet.Core.Type;
using Enums;
using UnityEditor;

[System.Serializable]
public class BlizSkillInfoGoogleSheet
{
    public int index;
    public eSkillType skillType;
    public int damage;
    public int distance;
    public int coolTime;
    public int duration;

}

public class SkillManager : Singleton<SkillManager>
{
    // [Header("게임 기본 데이터")]
    // public ScriptableObjectData mGameData;

    [SerializeField] public SkillDataContainer skillDataContainer;

    private Dictionary<eSkillType, List<BlizSkillInfoGoogleSheet>> mDicSkillData;
    ///<summary>
    /// 데이터 가져오기 함수
    ///</summary>
    public List<BlizSkillInfoGoogleSheet> mGetDontStopData(eSkillType _skill)
    {
        return mDicSkillData[_skill];
    }

    public bool TryGetSkillData(eSkillType skillType, int level, out BlizSkillInfoGoogleSheet result)
    {
        result = default;
        if (mDicSkillData.TryGetValue(skillType, out var list))
        {
                result = list[Mathf.Max(0, Mathf.Min(9, level))];
                return true;
        }

        return false;
    }
    public BlizSkillInfoGoogleSheet mSkillData(eSkillType skillType, int level, out BlizSkillInfoGoogleSheet result)
    {
        result = default;
        if (mDicSkillData.TryGetValue(skillType, out var list))
        {
            int index = Mathf.Max(0, Mathf.Min(10, level)); // level은 1부터 시작하므로 -1 보정
            if (list.Count > index)
            {
                result = list[index];
                return result;
            }
        }
        return null;
    }

    void Awake()
    {
        UnityGoogleSheet.LoadAllData();
        LoadData();
    }



    ///<summary>
    /// 데이터 로딩 
    /// TODO : 인디케이터 구현
    ///</summary>
    void LoadData()
    {
        if (mDicSkillData == null)
        {
            mDicSkillData = new();
        }

        mDicSkillData.Clear();
        int count = 0;
        foreach (var _data in BlizSkillInfo.BlizSkillData.BlizSkillDataList)
        {
            eSkillType _skillType = Constants.ConvertEnumData<eSkillType>(_data.skillType);

            BlizSkillInfoGoogleSheet _skillData = new BlizSkillInfoGoogleSheet()
            {
                index = count,
                skillType = _skillType,
                damage = _data.damage,
                distance = _data.distance,
                coolTime = _data.coolTime,
                duration = _data.duration,
            };

            if (mDicSkillData.ContainsKey(_skillType) == false)
            {
                List<BlizSkillInfoGoogleSheet> _list = new();
                _list.Add(_skillData);
                mDicSkillData.Add(_skillType, _list);
            }
            else
            {
                List<BlizSkillInfoGoogleSheet> _list = mDicSkillData[_skillType];
                _list.Add(_skillData);
            }

            // if (mDicSkillData.TryGetValue(_skillType, out var valueList))
            // {
            //     valueList.Add(_skillData);
            // }
            // else
            // {
            //     mDicSkillData[_skillType] = new List<BlizSkillInfoGoogleSheet> { _skillData };
            // }
            count++;
        }
#if UNITY_EDITOR
        // 스크립터블오브젝트로 저장
        SaveSkillDataToScriptableObject();
#endif
    }

    public void SaveSkillDataToScriptableObject(string path = "Assets/Resources/SkillDataContainer.asset")
    {
#if UNITY_EDITOR
        SkillDataContainer container = ScriptableObject.CreateInstance<SkillDataContainer>();

        foreach (var pair in mDicSkillData)
        {
            SkillGroup group = new SkillGroup
            {
                skillType = pair.Key,
                skillLevelsInfo = new List<BlizSkillInfoGoogleSheet>(pair.Value)
            };
            container.skillGroups.Add(group);
        }

        AssetDatabase.DeleteAsset(path); // 중복 방지
        AssetDatabase.CreateAsset(container, path);
        AssetDatabase.SaveAssets();

        if (skillDataContainer == null)
        {
            skillDataContainer = container;
        }

        Debug.Log($"[SkillManager] SkillDataContainer 저장 완료: {path}");
#endif
    }

}