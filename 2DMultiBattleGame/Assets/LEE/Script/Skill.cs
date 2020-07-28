using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스킬에 들어갈 스크립트
public class Skill : MonoBehaviour
{
    public int damage;              //데미지
    public float radius;            //범위

    //해당 범위안에 있는 플레이어들을 공격
    void Attack()
    {
        Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, radius);     //범위 안의 모든 콜라이더 가져옴

        foreach(Collider2D _coll in coll)
        {
            if(_coll.tag == "Player") //레이어가 플레이어면
                _coll.GetComponent<PlayerManager>().GetDamage(damage);//데미지를 줌
        }
    }

    //애니메이션 트리거 네트워크상에 있는 본인의 객체를 지움
    void OffObject()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
