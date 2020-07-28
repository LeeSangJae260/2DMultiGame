using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Option창의 메뉴들에게 들어갈 스크립트
public class ToggleBar : MonoBehaviour
{
    public Option option;       //옵션에 들어있는 이미지를 받기위한 변수
    public Image backGround;    //본인의 이미지를 바꿀 변수
    Toggle toggle;              //본인의 Toggle 컴퍼넌트를 담을 변수

    void Start()
    {
        toggle = GetComponent<Toggle>();
    }

    void Update()
    {
        if(toggle.isOn == true) //본인이 True가 된다면 이미지 체인지
            backGround.sprite = option.selectButton_Image;
        else
            backGround.sprite = option.button_Image;

    }
}
