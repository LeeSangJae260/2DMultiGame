using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//맵 밖으로 떨어질때 사용될 스크립트
public class DieArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            collision.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, 1000);
    }
}
