using System.Collections.Generic;
using UnityEngine;
using Enums;

[CreateAssetMenu(fileName = "SkillDataContainer", menuName = "Data/SkillDataContainer")]
public class SkillDataContainer : ScriptableObject
{
    public List<SkillGroup> skillGroups = new();
}

[System.Serializable]
public class SkillGroup
{
    public eSkillType skillType;
    public List<BlizSkillInfoGoogleSheet> skillLevelsInfo = new();
}
