using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//메인 씬에서 사용될 스크립트
public class MainManager : MonoBehaviour
{
    public Transform[] playerPosition;
    
    void Start()
    {
        CreataPlayer();
    }

    //해당 플레이어를 네트워크상에 소환할 함수
    public void CreataPlayer()
    {
        int localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        Transform spawnPosition = playerPosition[localPlayerIndex % playerPosition.Length];
        PhotonNetwork.Instantiate(PlayerData.player.name, spawnPosition.position, spawnPosition.localRotation);
    }
}
