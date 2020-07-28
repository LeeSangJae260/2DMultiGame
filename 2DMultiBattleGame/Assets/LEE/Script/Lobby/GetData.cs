using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//저장된 데이터를 받아오는 스크립트
public class GetData : MonoBehaviourPun
{
    public Image icon;
    public Text text;

    void Start()
    {
        ReadyPlayer r_player = FindObjectOfType<ReadyPlayer>();

        //본인이 방장인지 아닌지에 따라 텍스트를 바꾸고
        if (photonView.IsMine)
        {
            //본인이면 테두리를 빨간색으로 바꿈
            GetComponent<Image>().color = Color.red;
            if (PhotonNetwork.IsMasterClient)
                photonView.RPC("IsHost", RpcTarget.AllBuffered, null);
            else
                photonView.RPC("IsPlayer", RpcTarget.AllBuffered, null);
        }
        else
            GetComponent<Image>().color = Color.blue;

        transform.SetParent(r_player.transform, true);
        transform.localScale = Vector3.one;
    }

    [PunRPC]
    public void IsHost()
    {
        text.text = "HOST";
    }
    [PunRPC]
    public void IsPlayer()
    {
        text.text = "";
    }
}
