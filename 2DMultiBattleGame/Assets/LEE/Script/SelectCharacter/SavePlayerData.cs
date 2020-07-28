using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//선택된 플레이어의 데이터를 저장하는 스크립트
public class SavePlayerData : MonoBehaviour
{
    public GameObject character;  
    public GameObject icon;
    public Sprite iconSprite;

    public void SaveData()
    {
        PlayerData.player = character;
        PlayerData.iconButton = icon;
        PlayerData.icon = iconSprite;
    }
}
