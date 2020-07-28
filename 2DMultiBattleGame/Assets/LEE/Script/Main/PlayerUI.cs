using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//인 게임에서 본인의 체력과 마나, 이미지를 가져오고 컨트롤 하는 스크립트
public class PlayerUI : MonoBehaviour
{
    public PlayerManager player; //플레이어(본인)이 소환될때 세팅이 된다

    [Space(10)]
    public Image icon;           //얼굴 이미지
    public Slider hp;            //피통
    public Slider mp;            //마나

    [Space(10)]
    public Text hpText;          //체력 텍스트
    public Text mpText;          //마나 텍스트

    [Space(10)]
    public Image skill_Icon;      //스킬 아이콘
    [HideInInspector] public bool isCoolTime = true;     //스킬 쿨타임이 돌아 가는 상태인지
    [HideInInspector] public float currentTime = 0;       //스킬 쿨타임
    [HideInInspector] public float delayTime;             //딜레이 타임(실제 쿨타임)
  

    void Start()
    {
        icon.sprite = PlayerData.icon;
    }


    void Update()
    {
        if (player)
        {
            hp.value = Mathf.Lerp(hp.value, player.currHP, 0.5f); // 체력 자연스럽게 내려가기
            mp.value = Mathf.Lerp(mp.value, player.currMP, 0.5f); // 마나 자연스럽게 내려가기

            hpText.text = string.Format("{0:f0}", hp.value) + "/" + hp.maxValue;
            mpText.text = string.Format("{0:f0}", mp.value) + "/" + mp.maxValue;

            CoolTime();
        }
    }

    //스킬의 쿨타임 함수
    void CoolTime()
    {
        if (isCoolTime)
        {
            currentTime += Time.deltaTime;
            skill_Icon.fillAmount = currentTime / delayTime;

            if(currentTime >= delayTime)
            {
                isCoolTime = false;
                currentTime = 1f;
                skill_Icon.fillAmount = currentTime;
            }
        }
    }

}
