using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//option에 들어갈 스크립트
public class ButtonSelect : MonoBehaviour
{
    public Transform parent;            //자식 객체들을 가져올 부모 변수
    [Space(10)]
    GameObject[] option_Button;         //UI객체를 담을 배열
    public EventSystem eventSystem;     //UI표시를 위한 이벤트 시스템
    public AudioClip selectSound;       //선택 사운드
    public AudioClip clickSound;        //선택완료 사운드
    public int select = 0;              //현재 선택된 UI 번호

    [Space(10)]
    public GameObject opiont;
    bool isOption;

    AudioSource _audio;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.clip = selectSound;
        option_Button = new GameObject[parent.childCount]; //배열 초기화

        for (int i = 0; i < option_Button.Length; i++)     //자식객체을 배열에 추가
        {
            option_Button[i] = parent.GetChild(i).gameObject;
        }

        SelectedUI();                   //선택된 UI표시
        AnimControll();
    }

    void Update()
    {
        if (isOption == true)
        {
            eventSystem.SetSelectedGameObject(null);
            return;
        }

        SelectedUI();                   //선택된 UI표시

        //키 입력에 따라 선택될 UI 변경
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!(select - 1 < 0))
                select--;
            AudioPlay();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!(select + 1 > option_Button.Length - 1))
                select++;
            AudioPlay();
        }
        if (Input.anyKeyDown)
            AnimControll();

    }

    //이벤트 시스템의 Selected 설정 함수
    void SelectedUI()
    {
        if (!eventSystem.alreadySelecting)
            eventSystem.SetSelectedGameObject(option_Button[select]);
    }

    //UI애니메이션 컨트롤 함수
    void AnimControll()
    {
        for (int i = 0; i < option_Button.Length; i++)
        {
            if (i != select)
                option_Button[i].GetComponent<Animator>().SetBool("Select", false);
            else
                option_Button[i].GetComponent<Animator>().SetBool("Select", true);
        }
    }

    //오디오 플레이
    void AudioPlay()
    {
        _audio.clip = selectSound;
        _audio.Play();
    }

    //선택 사운드
    public void ButtonClick(bool isTrue)
    {
        opiont.SetActive(isTrue);
        isOption = isTrue;
    }

    //클릭하면 나올 사운드
    public void ClickSound()
    {
        _audio.clip = clickSound;
        _audio.Play();
    }

    //프로그램 종료
    public void Quit()
    {
        Application.Quit();
    }
}
