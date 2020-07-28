using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//로비 씬에 사용될 스크립트
public class LobbyManager : MonoBehaviour
{
    public Transform[] playerPosition;

    //플레이어 아이콘을 소환함.
    public void SpawnPlayer()
    {
        int localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        Transform spawnPosition = playerPosition[localPlayerIndex % playerPosition.Length];
        PhotonNetwork.Instantiate(PlayerData.iconButton.name, spawnPosition.position, spawnPosition.localRotation);
    }
}
