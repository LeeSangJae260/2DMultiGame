using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//포톤 네트워크에서 플레이 준비를 완료한 사람의 수
public class ReadyPlayer : MonoBehaviour
{
    [HideInInspector]
    public static int readyPlayer = 0;
}
