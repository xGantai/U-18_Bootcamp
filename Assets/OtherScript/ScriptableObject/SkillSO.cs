using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Skill",menuName ="ScriptableObject/SkillSystem/Skill")]
public class SkillSO : ScriptableObject
{
    public Sprite Thumbnail;
    public string Name;
    [field: TextArea]
    public string Description;
    public int SkillPower;
    public float CoolDown;
    public int SkillPoint;
    public int SkillLevel;
    public SkillTypes SkillType;
    public SkillStat SkillStatus;
    public GameObject SkillPrefabs;


    public enum SkillTypes
    {
        None,
        BladeFight,
        SkillShot
    }
    public enum SkillStat
    {
        Passive,
        Active
    }
}
