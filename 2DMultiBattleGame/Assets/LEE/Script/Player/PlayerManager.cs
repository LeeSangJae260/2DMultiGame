using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//플레이어에게 들어갈 스크립트(주로 움직임, 공격등등 )
public class PlayerManager : PlayerStatus, IPunObservable
{
    [HideInInspector] public bool isDie;            //플레이어가 죽었는지 관리할 변수
    [HideInInspector] public bool action;           //플레이어가 행동을 하고 있는지
    [HideInInspector] public string playerName;     //플레이어 이름(Lank시스템 이용)
    [HideInInspector] public int currHP;            //스크립트용 HP
    [HideInInspector] public int currMP;            //스크립트용 MP
    [HideInInspector] public Animator anim;         //애니메이터

    [Space(10)]
    public Camera p_camera;                         //카메라

    [Space(10)]
    public Slider HP_Slider;                        //HP슬라이더
    public Transform attackPivot;                   //공격 범위(BoxCollider2D ,위치)

    bool isAttack;                                  //공격 가능한 상태인가

    Rigidbody2D rigid;                              //RigidBody

    [Space(10)]
    public float setAttackTime;                     //공격 간격
    public bool isGround;                           //지금 바닥에 있는가?
    public float jumpTime;                          //공중에 있을 시간 설정
    bool isJumping;                                 //지금 점프중인가?
    float jumpTimeCounter;                          //공중에 있을 시간 체크

    float attackTime;                               //공격 쿨타임
    float scaleX;                                   //본인의 바라보는 방향을 정할 스케일 값
    float dirX;                                     //본인의 이동 방향값을 받음

    Vector3 scale;                                  //본인의 크기

    PhotonView myPv;
    StopWatch watch;                                //타이머스크립트

    AudioSource _audio;                             //사운드

    //위치 정보를 송수신할 때 사용할 변수 선언 및 초깃값 설정
    Vector3 currPos;
    Vector3 currScal;
    Quaternion currRot;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale);
            stream.SendNext(transform.rotation);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currScal = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Start()
    {
        Invoke("InvokeIsAttack", 3f);       //3초뒤 공격 가능
        Init();  
    }

    //초기화 함수
    void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        myPv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        if (FindObjectOfType<StopWatch>())
        {
            watch = FindObjectOfType<StopWatch>();
            watch.player.Add(this);
        }

        playerName = "Player" + PhotonNetwork.LocalPlayer.ActorNumber;      //랭킹용 플레이어 이름  

        currHP = HP;
        currMP = MP;

        HP_Slider.maxValue = currHP;
        HP_Slider.value = currHP;

        scale = transform.localScale;
        scaleX = Mathf.Abs(transform.localScale.x);

        currPos = transform.position;
        currRot = transform.rotation;
        currScal = transform.localScale;

        if (myPv.IsMine)
        {
            p_camera.enabled = true;
            PlayerUISetting();                      //본인의 UI 설정
            HP_Slider.gameObject.SetActive(false);  //플레이어 캐릭터 위의 체력 끄기(본인은 UI로 보임)
        }
        else
        {
            p_camera.enabled = false;
        }
    }

    //rigidbody 때문에 
    void FixedUpdate()
    {
        HP_Slider.value = Mathf.Lerp(HP_Slider.value, currHP, 0.5f); // 체력 자연스럽게 내려가기

        if (!myPv.IsMine || isDie == true)
            return;

        Move();
    }

    void Update()
    {
        if (action == true || isDie == true)
            return;

        if (!myPv.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime * 3f);
            transform.localScale = Vector3.Lerp(transform.localScale, currScal, Time.deltaTime * 3f);
            transform.rotation = Quaternion.Slerp(transform.rotation, currRot, Time.deltaTime * 3f);
            return;
        }

        Jump();
        Attack();
        Die();
    }

    //움직임 함수
    void Move()
    {
        dirX = Input.GetAxis("Horizontal"); //키 입력 값을 받아옴

        if(dirX == 0)
            anim.SetBool("Move", false);
        else
            anim.SetBool("Move", true);

        if (action == false)
        {
            Direction();
            rigid.velocity = new Vector2(dirX * SPEED, rigid.velocity.y);
        }
        else
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
    }

    //이동 방향에따라 캐릭터 방향도 바뀜
    public void Direction()
    {
        if (dirX > 0)
        {
            scale.x = scaleX;
        }
        if (dirX < 0)
        {
            scale.x = -scaleX;
        }

        transform.localScale = scale;
    }

    //점프 함수
    void Jump()
    {
        if (isGround == true)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                isJumping = true;                               //추가 점프가 가은한 상태(2단 점프는 아님)
                anim.SetBool("Jump", true);
                jumpTimeCounter = jumpTime;                     //점프시간 초기화
                myPv.RPC("JumpRPC", RpcTarget.All);             //점프
            }
        }

        if (Input.GetKey(KeyCode.X) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                myPv.RPC("JumpRPC", RpcTarget.All);             //점프
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }

        }
        if (Input.GetKeyUp(KeyCode.X))
            isJumping = false;
    }
    //포톤용
    [PunRPC]
    void JumpRPC()
    {
        rigid.velocity = Vector2.up * JUMPFORCE;        //점프
    }

    //공격 함수
    void Attack()
    {
        if (isAttack && attackTime > setAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                anim.SetTrigger("Attack");
                attackTime = 0f;                //공격 시간 초기화
                action = true;                  //현재 행동 중
                Collider2D[] coll = Physics2D.OverlapBoxAll(attackPivot.position, attackPivot.GetComponent<BoxCollider2D>().size, 0);
                foreach (Collider2D _coll in coll)  //범위 안의 플레이어 공격 (본인은 제외)
                {
                    if (_coll.tag == "Player" && _coll.GetComponent<PhotonView>().IsMine == false)
                    {
                        _coll.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, ATK);
                    }
                }
            }
        }
        attackTime += Time.deltaTime;
    }
    //공격 가능한지 설정할 함수
    void IsAttack(bool isTrue)
    {
        isAttack = isTrue;
    }

    void InvokeIsAttack()
    {
        IsAttack(true);
    }

    //피격 함수
    [PunRPC]
    public void GetDamage(int damage)
    {
        if (isDie != true)
        {
            currHP -= damage;
            action = true;
            anim.SetTrigger("Hurt");
            _audio.Play();
        }
    }

    //애니메이션용 스크립트(움직이게 만듬)
    public void Action()
    {
        action = false;
    }

    //본인의 UI를 찾아서 셋팅할 함수
    void PlayerUISetting()
    {
        if (FindObjectOfType<PlayerUI>())
        {
            PlayerUI p_UI = FindObjectOfType<PlayerUI>();
            //UI의 체력 설정
            p_UI.player = this;
            p_UI.hp.maxValue = currHP;
            p_UI.hp.value = currHP;
            //UI의 마나 설정
            p_UI.mp.maxValue = currMP;
            p_UI.mp.value = currMP;
            //UI의 스킬 아이콘 설정
            p_UI.skill_Icon.sprite = PlayerData.skill_Icon;
        }
    }

    //본인을 꺼버린다(애니메이션 트리거용)
    public void OffObjct()
    {
        gameObject.SetActive(false);
    }

    //죽었을떄의 행동
    void Die()
    {
        if(currHP <= 0)
        {
            isDie = true;
            anim.SetTrigger("Die");
        }
    }

    //죽었을때 사용될 애니메이션 트리거
    public void DieTrigger()
    {
        if (myPv.IsMine)
        {
            watch.watchingMode = true;      //관전 모드 on
        }

        GetComponent<SkillManager>().enabled = false;
        watch.ShowCamera();             //현재 살아있는 플레이어 보여줌
        watch.CheckPlayerHP();          //죽은 플레이어 + 1
        watch.PublicGameEnd();          //본인이 죽음으로서 게임이 끝나는가?
    }
}
