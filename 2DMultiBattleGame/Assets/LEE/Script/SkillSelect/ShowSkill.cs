using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스킬 선택씬에 사용된 스크립트
public class ShowSkill : MonoBehaviour
{
    public GameObject[] skill;      //스킬들의 프리펩을 담을 배열
    public Animator anim;           //애니메이터
    SkillManager player_skill;      //해당 플레이어의 스킬 매니저
    void Awake()
    {
        player_skill = FindObjectOfType<SkillManager>();    //소환된 플레이어의 스킬매니저를 찾아서 가져옴
        anim = player_skill.GetComponent<Animator>();       //해당 플레이어의 애니메이터를 가져옴
    }

    //스킬 생성
    public void CreateShowSkill(int select)
    {
        player_skill.skill_Prefab = skill[select];
    }
}
