using System;
using System.Collections;
using System.IO;
// using Newtonsoft.Json.Linq;
using System.Data;
using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;
// using Newtonsoft.Json;
using Enums;


public class BlizSkillInfoCvs
{
    public int index;
    public eSkillType skillType;
    public int damage;
    public int distance;
    public int coolTime;
    public int duration;

}


///<summary>
/// cvs 파일 로드시 사용되는 스크립트 입니다 
///</summary>
public class DataControllManager : Singleton<DataControllManager>
{
    
    public DataReader dataReader;
    /***************************************************************************/
    // 스킬 레벨 정보 
    private Dictionary<eSkillType, List<BlizSkillInfoCvs>> mDicSkillInfo;
    public List<BlizSkillInfoCvs> GetSkillInfo(eSkillType _key)
    {
        return mDicSkillInfo[_key];
    }
    /***************************************************************************/


    ///<summary>
    /// 데이터 생성 
    ///</summary>
    public void initRelease()
    {

        if (mDicSkillInfo == null)
        {
            mDicSkillInfo = new Dictionary<eSkillType, List<BlizSkillInfoCvs>>();
        }
    }

    ///<summary>
    /// 데이터 초기화
    ///</summary>
    public void initData()
    {
        if (mDicSkillInfo is not null)
        {
            mDicSkillInfo.Clear();
        }
    }


    void Awake()
    {
        initRelease();
        initData();

        ReadStageInfo();
    }


    void ReadStageInfo()
    {
        dataReader.ReadCvsData("BlizSkillInfo", OnBlizSkillInfoDataReadLine);
    }

    void OnBlizSkillInfoDataReadLine(string[] lines)
    {
        try
        {
            // 첫 번째 줄은 설명 건너뜁니다.
            // 두번째 줄은 해더 건너 뜀
            for (int i = 3; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = line.Split(',');

                BlizSkillInfoCvs _info = new BlizSkillInfoCvs();

                // 순번,스킬타입,공격력,범위,쿨타임,지속시간,설명
                _info.index = int.Parse(fields[0]);
                _info.skillType = Constants.ConvertEnumData<eSkillType>(fields[1]);
                _info.damage = int.Parse(fields[2]);
                _info.distance = int.Parse(fields[3]);
                _info.coolTime = int.Parse(fields[4]);
                _info.duration = int.Parse(fields[5]);

                List<BlizSkillInfoCvs> _list = new List<BlizSkillInfoCvs>();
                if (mDicSkillInfo.ContainsKey(_info.skillType) == false)
                {
                    _list.Add(_info);
                    mDicSkillInfo.Add(_info.skillType, _list);
                }
                else
                {
                    _list = mDicSkillInfo[_info.skillType];
                    _list.Add(_info);
                }

            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("OnBulletPatternInfoDataReadLine error : " + e.ToString());
        }
    }
}