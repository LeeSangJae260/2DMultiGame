using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//로비씬에서 레디 버튼을 누르거나 시작을 누를 수 있게 해줄 스크립트
public class ReadyButton : MonoBehaviourPun
{
    public GameObject start;        //스타트 버튼
    public GameObject ready;        //레디 버튼
    [Space(10)]
    public Text player;             //준비됬다면 표시할 텍스트

    bool b_ready;                   //준비완료?

    void Start()
    {
        if(photonView.IsMine)
            SetButton();
    }

    //버튼 셋팅
    public void SetButton()
    {
        if (PhotonNetwork.IsMasterClient)//방장이라면 Start 버튼이 아니면 Ready버튼이 나옴
            start.SetActive(true);
        else
            ready.SetActive(true);

        //플레이어의 위치에 따라 버튼의 위치를 가운데로 옴김
        transform.position = new Vector3(transform.position.x - transform.parent.position.x, transform.position.y, 0f);
    }

    //Ready한 플레이어가 인원 수와 맞다면 시작이 가능
    public void BattleStart()
    {
        if (PhotonNetwork.IsMasterClient)
            if (PhotonNetwork.PlayerList.Length - 1 == ReadyPlayer.readyPlayer)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;                //게임을 시작하면 방 문을 닫음(다른 유저가 검색을 못함: 못들어옴)
                PhotonNetwork.LoadLevel("Main" + Random.Range(1, 3));       //모든 유저 씬 이동 1~2맵중 랜덤   
            }
    }

    public void ReadyGame()
    {
        photonView.RPC("RPCReadyGame", RpcTarget.All, null);
    }

    //Ready버튼을 누르면 준비완료 혹은 준비 취소 가능한 함수
    [PunRPC]
    void RPCReadyGame()
    {
        b_ready = !b_ready;

        if (b_ready == true)
        {
            photonView.RPC("IsReady", RpcTarget.All, null);
            ReadyPlayer.readyPlayer++;
        }
        else if (b_ready == false)
        {
            photonView.RPC("IsNotReady", RpcTarget.All, null);
            ReadyPlayer.readyPlayer--;
        }
    }

    //텍스트 변경 함수
    [PunRPC]
    public void IsReady()
    {
        player.text = "Ready!";
    }
    [PunRPC]
    public void IsNotReady()
    {
        player.text = "";
    }
}
