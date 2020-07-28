using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//Title씬에 있는 플레이어에게 들어갈 스크립트( 보여주기용 )
public class TitlePlayerManager : PlayerStatus
{ 
    Animator anim;                                  //애니메이터를 받음

    Rigidbody2D rigid;                              //본인의 Rigidbody를 받는다

    [Space(10)]
    public bool isGround;                           //지금 바닥에 있는가?
    public float jumpTime;                          //공중에 있을 시간 설정
    bool isJumping;                                 //지금 점프중인가?
    float jumpTimeCounter;                          //공중에 있을 시간 체크

    float scaleX;                                   //본인의 바라보는 방향을 정할 스케일 값
    float dirX;                                     //본인의 이동 방향값을 받음

    Vector3 scale;                                  //본인의 크기

    void Start()
    {
        Init();
    }

    //초기화 함수
    void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        scale = transform.localScale;
        scaleX = Mathf.Abs(transform.localScale.x);     //방향이 어떠하든 절대값으로 받아옴
    }

    void FixedUpdate()      //RigidBody를 균등한 속도로 사용하기 위해 씀
    {
        Move();
    }

    void Update()           //똑같이 RigidBody를 쓰지만 키 입력이 무시당하는 경우가 있어서 일반 Update문에 사용
    {
        Jump();
    }

    //움직임 함수
    void Move()
    {
        dirX = Input.GetAxis("Horizontal"); //키 입력 값을 받아옴

        if (dirX == 0)
            anim.SetBool("Move", false);
        else
            anim.SetBool("Move", true);

        Direction();
        rigid.velocity = new Vector2(dirX * SPEED, rigid.velocity.y);
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
                rigid.velocity = Vector2.up * JUMPFORCE;        //점프
            }
        }

        if (Input.GetKey(KeyCode.X) && isJumping == true)
        {
            if (jumpTimeCounter > 0)                            //정해진 시간만큼 점프함
            {
                rigid.velocity = Vector2.up * JUMPFORCE;        //점프
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

    //바닥에 착지했는가 혹은 바닥에 있는가
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
            anim.SetBool("Jump", false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = false;
        }
    }
}
