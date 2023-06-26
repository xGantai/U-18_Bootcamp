using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfo", menuName = "ScriptableObject/Character")]
public class CharactersSO : ScriptableObject
{
    public string Name;
    public GameObject CharacterPrefab;
    public CharacterClass Classes;
    public SkillSO CurrentPassive;
    public SkillSO CurrentActive1;
    public SkillSO CurrentActive2;
    public SkillSO CurrentActive3;
    public SkillTreeSO SkillTree;

    public enum CharacterClass
    {
        Warrior,
        Thief,
        Wizard,
        Priest
    }
}