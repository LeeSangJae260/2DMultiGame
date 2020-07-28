using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//스킬 선택씬에 사용할 스크립트
public class SkillIconSetting : MonoBehaviour
{
    public Transform[] skill_Icon;       //생성할 스킬 아이콘 배열

    [Space(5)]
    public ShowSkill showSkill;     //스킬을 보여주기 위한 스크립트를 담을 변수
    public EventSystem eventSystem;         //화면에 표시하기 위한 이벤트 시스템 
    public int select = 0;                  //현재 선택된 UI
    public Transform skillPivot;           //스킬 소환 위치

    AudioSource _audio;
    GameObject[] _skill_Icon;            //이벤트 시스템 작동을 위한 배열(프리펩으로 받으면 생성된 오브젝트가 아니기 때문에 UI표시 안됨)

    void Awake()
    {
        _skill_Icon = new GameObject[skill_Icon.Length]; //배열 초기화
        CreateSkill_ICon();              //스킬 아이콘 생성
    }

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        SelectedUI();                       //스킬 아이콘 Select 표시
        ShowSkill();
    }

    void Update()
    {
        SelectedUI();                       //스킬 아이콘 Select 표시

        //키 입력에 따라 선택될 UI 변경
        MoveSelectUp(1, KeyCode.RightArrow);
        MoveSelectDown(1, KeyCode.LeftArrow);

        MoveSelectUp(6, KeyCode.DownArrow);
        MoveSelectDown(6, KeyCode.UpArrow);
        if (Input.anyKeyDown)
            ShowSkill();
    }

    //int값 변경
    void MoveSelectUp(int index, KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            if (!(select + index > skill_Icon.Length - 1))
                select += index;
            else
                select = skill_Icon.Length - 1;
            _audio.Play();
        }
    }
    void MoveSelectDown(int index, KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            if (!(select - index < 0))
                select -= index;
            else
                select = 0;
            _audio.Play();
        }
    }

    //Show스킬 생성 스크립트
    void ShowSkill()
    {
        showSkill.CreateShowSkill(select);
        showSkill.anim.SetTrigger("Attack");
        Instantiate(showSkill.skill[select] ,skillPivot.position, skillPivot.rotation);
    }

    //이벤트 시스템의 Selected 설정 함수
    void SelectedUI()
    {
        if (!eventSystem.alreadySelecting)
            eventSystem.SetSelectedGameObject(_skill_Icon[select]);
    }

    //스킬 아이콘 생성 함수
    void CreateSkill_ICon()
    {
        for (int i = 0; i < skill_Icon.Length; i++)
        {
            Transform icon = Instantiate(skill_Icon[i], Vector3.zero, Quaternion.identity) as Transform;
            icon.name = "Skill_Icon " + i;
            icon.SetParent(transform, false);
            _skill_Icon[i] = icon.gameObject;
        }
    }
}
