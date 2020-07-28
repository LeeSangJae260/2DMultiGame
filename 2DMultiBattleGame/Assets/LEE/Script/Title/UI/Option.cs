using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Title 씬의 옵션 스크립트
public class Option : MonoBehaviour
{
    [Space(10)]
    public Sprite selectButton_Image;       //선택된 기능이라 표시할 이미지
    public Sprite button_Image;             //기본 이미지
    public Slider audioSlider;              //오디오 사운드 크기 설정할 슬라이더
    public AudioSource _audio;              //오디오소스
    public AudioSource _BackGroundaudio;    //배경 오디오 사운드(BGM)

    [Space(10)]
    public Toggle audio_Toggle;             //오디오 옵션 토글
    public Toggle quit_Toggle;              //옵션창 끄기 토글
    public Toggle windowSize_Toggle;        //윈도우 사이즈 토글
    public Text windowSize_Txt;             //윈도우 사이즈 표시
    public string[] windowSize;             //사이즈 텍스트 표시
    public int[] windowX;                   //실제 정용될 X크기
    public int[] windowY;                   //실제 정용될 Y크기

    int selectOption = 0;                   //현재 선택된 옵션기능

    public static int select = 2;           //윈도우 사이즈 디폴트 값
    public static float audioVolume = .5f;  //볼륨 디폴트 값

    void Start()
    {
        windowSize_Txt.text = windowSize[select];
        audioSlider.value = audioVolume;
    }

    void OnEnable()
    {
        _audio.clip = _audio.GetComponent<ButtonSelect>().selectSound;
    }

    void Update()
    {
        //키입력을 통해 선택
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!(selectOption - 1 < 0))
                selectOption--;
            _audio.Play();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!(selectOption + 1 > 2))
                selectOption++;
            _audio.Play();
        }

        SelectedOption();
        SetVolume();
        WindowSetSize();
        SetQuit();
    }


    //선택된 기능 표시
    void SelectedOption()
    {
        switch (selectOption)
        {
            case 0:
                audio_Toggle.isOn = true;
                break;
            case 1:
                windowSize_Toggle.isOn = true;
                break;
            case 2:
                quit_Toggle.isOn = true;
                break;
        }
    }

    //옵션 창 끄기
    void SetQuit()
    {
        if(quit_Toggle.isOn == true)
        {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                FindObjectOfType<ButtonSelect>().ButtonClick(false);
            }
        }
    }

    //오디오 설정
    void SetVolume()
    {
        if (audio_Toggle.isOn == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                audioSlider.value -= .1f;
                _audio.Play();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                audioSlider.value += .1f;
                _audio.Play();
            }

            audioVolume = audioSlider.value;
            _audio.volume = audioVolume;
            _BackGroundaudio.volume = audioVolume;
        }
    }

    //윈도우 사이즈 설정
    void WindowSetSize()
    {
        if (windowSize_Toggle.isOn == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (!(select - 1 < 0))
                    select--;
                _audio.Play();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!(select + 1 > windowSize.Length - 1))
                    select++;
                _audio.Play();
            }

            windowSize_Txt.text = windowSize[select];
            Screen.SetResolution(windowX[select], windowY[select], false);
        }
    }
}
