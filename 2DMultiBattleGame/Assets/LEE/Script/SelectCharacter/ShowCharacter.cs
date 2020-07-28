using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//보여주기용 캐릭터 생성 스크립트
public class ShowCharacter : MonoBehaviour
{
    public GameObject[] character;      //캐릭터들의 프리펩을 담을 배열
    public Text[] status;               //스테이터스 표시 텍스트를 담을 배열
    public bool isPlayerSetting;        //선택된 플레이어를 소환 할 것인가?

    GameObject player;                  //소환된 플레이어캐릭터

    void Awake()
    {
        if (isPlayerSetting)
            CreateShowChar(PlayerData.player);
    }

    //보여주기용 캐릭터 생성 및 캐릭터의 스텟 표시
    public void CreateShowChar(int select)
    {
        if (player != null)
            Destroy(player);

        PlayerStatus player_status = character[select].GetComponent<PlayerStatus>();

        status[0].text = "HP : " + player_status.HP;
        status[1].text = "MP : " + player_status.MP;
        status[2].text = "ATK : " + player_status.ATK;
        status[3].text = "SPEED : " + player_status.SPEED;

        if (transform.Find(character[select].name+"(Clone)"))
            DestroyImmediate(transform.Find(character[select].name+ "(Clone)").gameObject);

        Transform obj = Instantiate(character[select], transform.position, Quaternion.identity).transform;
        obj.GetComponent<PlayerManager>().enabled = false;      //스크립트를 끔(단순 보여주기용)
        obj.GetComponent<SkillManager>().enabled = false;       //스크립트를 끔(단순 보여주기용)
        player = obj.gameObject;
    }

    //플레이어 데이터에 값이 있다면 실행 시킬 함수
    public void CreateShowChar(GameObject prefab)
    {
        Transform obj = Instantiate(prefab, transform.position, Quaternion.identity).transform;
        obj.GetComponent<PlayerManager>().enabled = false;      //스크립트를 끔(단순 보여주기용)
        obj.GetComponent<Rigidbody2D>().isKinematic = true;     //물리현상 무시
        obj.GetChild(1).gameObject.SetActive(false);            //일부 기능을 끔
        obj.GetChild(2).gameObject.SetActive(false);            //일부 기능을 끔
    }
}
