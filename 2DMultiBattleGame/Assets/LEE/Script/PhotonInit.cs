using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//포톤 접속 스크립트
public class PhotonInit : MonoBehaviourPunCallbacks
{
    //버전 정보
    public string version = "1.0";

    void Awake()
    {
        PhotonNetwork.GameVersion = version;
        //포톤 클라우드 접속
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //포톤 클라우드에 정상적으로 접속한 후 로비에 입장하면 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("entered LObby !");
        PhotonNetwork.JoinRandomRoom();
    }

    //무작위 룸 접속에 실패한 경우 호출되는 콜백 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Nope Room !");
        //룸 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4});
        
    }

    //룸에 입장하면 호출되는 콜백함수
    public override void OnJoinedRoom()
    {
        Debug.Log("Enter Room");
        GetComponent<LobbyManager>().SpawnPlayer();
    }
}
