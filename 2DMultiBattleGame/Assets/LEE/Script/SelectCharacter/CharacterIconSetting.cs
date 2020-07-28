using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Scroll View - Content 에 들어갈 스크립트
public class CharacterIconSetting : MonoBehaviour
{
    public Transform[] characterIcon;       //생성할 캐릭터 아이콘 배열

    [Space(5)]
    public ShowCharacter showCharacter;     //캐릭터를 보여주기 위한 스크립트를 담을 변수
    public EventSystem eventSystem;         //화면에 표시하기 위한 이벤트 시스템 
    public int select = 0;    //현재 선택된 UI

    AudioSource _audio;

    GameObject[] _characterIcon;            //이벤트 시스템 작동을 위한 배열(프리펩으로 받으면 생성된 오브젝트가 아니기 때문에 UI표시 안됨)

    void Awake()
    {
        _characterIcon = new GameObject[characterIcon.Length]; //배열 초기화
        CreateCharacterICon();              //캐릭터 아이콘 생성
    }

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        SelectedUI();                       //캐릭터 아이콘 Select 표시
        ShowCharacter();
    }

    void Update()
    {
        SelectedUI();                       //캐릭터 아이콘 Select 표시

        //키 입력에 따라 선택될 UI 변경
        MoveSelectUp(1, KeyCode.RightArrow);
        MoveSelectDown(1, KeyCode.LeftArrow);

        MoveSelectUp(6, KeyCode.DownArrow);
        MoveSelectDown(6, KeyCode.UpArrow);
        if (Input.anyKeyDown)
            ShowCharacter();
    }

    //int값 변경
    void MoveSelectUp(int index, KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            if (!(select + index > characterIcon.Length - 1))
                select += index;
            else
                select = characterIcon.Length - 1;
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

    //Show캐릭터 생성 스크립트
    void ShowCharacter()
    {
        showCharacter.CreateShowChar(select);
    }

    //이벤트 시스템의 Selected 설정 함수
    void SelectedUI()
    {
        if (!eventSystem.alreadySelecting)
            eventSystem.SetSelectedGameObject(_characterIcon[select]);
    }

    //캐릭터 아이콘 생성 함수
    void CreateCharacterICon()
    {
        for (int i = 0; i < characterIcon.Length; i++)
        {
            Transform icon = Instantiate(characterIcon[i], Vector3.zero, Quaternion.identity) as Transform;
            icon.name = "Character " + i;
            icon.SetParent(transform, false);
            _characterIcon[i] = icon.gameObject;
        }
    }
}
