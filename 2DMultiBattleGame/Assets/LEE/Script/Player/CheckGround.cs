using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어의 착지가 됬는가를 표시할 함수
public class CheckGround : MonoBehaviour
{
    public PlayerManager player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            player.isGround = true;
            player.anim.SetBool("Jump", false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            player.isGround = false;
        }
    }
}
