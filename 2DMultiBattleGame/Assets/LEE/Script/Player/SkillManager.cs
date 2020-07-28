using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어에게 들어갈 스크립트
public class SkillManager : MonoBehaviourPun
{
    public KeyCode skill_Key;       //스킬 사용할 키 설정
    public GameObject skill_Prefab; //스킬 프리팹
    public Transform skill_Pivot;   //스킬 소환 지점
    public float setCoolTime;       //스킬 쿨타임

    Animator anim;                  //애니메이터
    PlayerManager player;           //플레이어
    PlayerUI p_UI;                  //인게임안의 본인의 UI
    [HideInInspector] public float coolTime;    //스크립트용 스킬 쿨타임

    void Start()
    {
        anim = GetComponent<Animator>();    //애니메이터 셋팅
        player = GetComponent<PlayerManager>(); //플레이어 매니저를 받음

        if (!photonView.IsMine)
            return;

        skill_Prefab = PlayerData.skill;    //스킬 프리펩 받음
        PlayerUISetting();                  //playerUI 세팅
    }

    //본인의 UI를 찾아서 셋팅할 함수
    void PlayerUISetting()
    {
        if (FindObjectOfType<PlayerUI>())       //해당 스크립트가 있다면 실행
        {
            p_UI = FindObjectOfType<PlayerUI>();
            p_UI.delayTime = setCoolTime;
        }
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (coolTime >= setCoolTime && Input.GetKeyDown(skill_Key) && player.currMP > 0)    //설정된 키를 누르면 실행
        {
            anim.SetTrigger("skill");       //스킬 애니메이션 실형
            coolTime = 0;
            p_UI.currentTime = 0f;          //스킬의 쿨타임 초기화 돌림
            player.currMP -= 20;            //마나 소모
            p_UI.isCoolTime = true;         //스킬 아이콘의 쿨타임을 돌림
            player.action = true;           //본인이 행동중이라 표시
        }
        coolTime += Time.deltaTime;
    }

    public void UseSkill()                  //스킬애니메이션 실행시 작동, 스킬 오브젝트 소환.
    {
        PhotonNetwork.Instantiate(skill_Prefab.name, skill_Pivot.position, Quaternion.identity); //네트워크 소환  
    }

    private void OnDrawGizmos()             //스킬의 소환 위치 기즈모
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(skill_Pivot.position, new Vector2(1f, 1f));
    }
}
