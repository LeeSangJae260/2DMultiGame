using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

//메인 씬에서 타이머 스크립트
public class StopWatch : MonoBehaviour
{
    public bool watchingMode;               //본인이 죽었다면 관전모드로 바꿈
    public Text timer;                      //게임의 시간 Text 객체
    public GameObject QuitButton;           //게임이 끝났을때 나올 버튼 UI
    public Camera mainCamera;
    public List<PlayerManager> player;      //플레이어들의 정보를 담을 리스트

    float time = 99f;                       //총 플레이시간 
    bool startTimer = false;                //게임이 시작 되었는가

    int count;                              //죽은 플레이어의 수(죽은 플레이어가 3명이면 게임이 끝남)
    public int cameraCount;                 //카메라 이동용 변수

    void Start()
    {
        Invoke("StartTimer", 3f);
        player = new List<PlayerManager>();
    }

    void Update()
    {
        if (startTimer)
        {
            time -= Time.deltaTime;
            timer.text = string.Format("{0:f0}", time);
        }
        else
            return;

        if(time <= 0)
            GameEnd();

        if (watchingMode == true)
            CameraControll();
    }

    //3초 뒤에 타이머를 작동
    void StartTimer()
    {
        startTimer = true;
    }

    //게임이 끝나면 실행될 함수
    void GameEnd()
    {
        startTimer = false;                                         //타이머를 멈춤
        QuitButton.SetActive(true);                                 //Title로 나갈 수 있는 버튼을 킴
        for (int i = 0; i < player.Count; i++)
        {
            player[i].enabled = false;                              //게임이 끝나서 행동들을 막음
            player[i].GetComponent<SkillManager>().enabled = false;
        }
    }

    //게임을 끝낼지에 대한 체크용
    public void PublicGameEnd()
    {
        if (count >= player.Count - 1)
        {
            GameEnd();
        }
    }

    //플레이어들의 체력을 확인(1명만 남으면 게임이 끝남)
    public void CheckPlayerHP()
    {
        count++;
    }

    //죽었을때 관전모드
    void CameraControll()
    {
        //키 입력에 따라 선택될 카메라 변경
        MoveSelectUp(1, KeyCode.LeftArrow);
        MoveSelectUp(1, KeyCode.DownArrow);

        MoveSelectDown(1, KeyCode.RightArrow);
        MoveSelectDown(1, KeyCode.UpArrow);

        if (Input.anyKeyDown)
            SelectCamera(cameraCount);

        ThisCameraIsDie();
    }

    //키입력을 받아 카메라 이동 함수
    void MoveSelectUp(int index, KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            if (!(cameraCount + index > player.Count - 1) && player[cameraCount + index].isDie != true)
                cameraCount += index;
        }
    }
    void MoveSelectDown(int index, KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            if (!(cameraCount - index < 0) && player[cameraCount - index].isDie != true)
                cameraCount -= index;
        }
    }

    //죽었을때 살아있는 플레이어의 카메라를 킴
    public void ShowCamera()
    {
        for (int i = 0; i < player.Count; i++)
        {
            if (player[i].isDie == false)
            {
                SelectCamera(i);
                break;
            }
        }
    }

    //현재 보고있는 유저가 죽으면 다른 유저로 이동
    void ThisCameraIsDie()
    {
        if (player[cameraCount].p_camera.enabled == false)
            ShowCamera();
    }

    //카메라 선택
    public void SelectCamera(int index)
    {
        for (int i = 0; i < player.Count; i++)
        {
            player[i].p_camera.enabled = false;
        }

        player[index].p_camera.enabled = true;
    }

    //나가기버튼을 누르면 방에서 나가고 서버연결을 끊음
    public void QuitNetwork()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        ReadyPlayer.readyPlayer = 0;    //레디 플레이어 초기화
        Invoke("GoTitle", .1f);         //바로 이동하면 포톤 에러 생성
    }

    void GoTitle()
    {
        SceneManager.LoadScene(0);
    }
}
