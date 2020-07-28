using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//선택한 스킬을 저장한다.
public class SaveSkill : MonoBehaviour
{
    public GameObject skill;
    public Sprite skill_Icon;

    public void SaveSkillPrefab()
    {
        PlayerData.skill = skill;
        PlayerData.skill_Icon = skill_Icon;
    }
}
