using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMover : MonoBehaviourPun
{
    [SerializeField] public float curSpeed = 0f;
    [SerializeField] public float originSpeed; // 이동속도
    [SerializeField] public float moveSpeed; // 이동속도
    [SerializeField] public float rotateSpeed; // 회전속도


    [SerializeField] public float speedIncrease; // 이동속도 증가율
    [SerializeField] public float speedChangeDuration; // 이동속도 변경 시간

    [SerializeField] private float startSpeedChangeTIme; // 이동속도 증가 시작 시간
    [SerializeField] private float gravity;

    [SerializeField] private float pullForce; // 잡아당기는 힘

    private Animator anim;
    private CharacterController controller;
    private Rigidbody rigid;
    private PlayerInput playerInput;

    [SerializeField] GameObject pullingPlayer;
    [SerializeField] private Transform lookRotation;


    private Vector3 inputDir;
    Vector3 fowarVec;
    Vector3 rightVec;


    private float ySpeed;

    private bool isPulling = false; // PlayerPuller 클래스로부터 전달받은 값
    // private bool isPushing = false; // PlayerPuller 클래스로부터 전달받은 값

    private bool isSpeedUp = false;
    private bool isSpeedDown = false;
    private bool isStun = false;
    bool haveControll = false;


    void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        rigid = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

    }

    void Start()
    {
        if (!photonView.IsMine)
            Destroy(playerInput);

        lookRotation.localPosition = new Vector3(0, 0, 0.2f);
        ySpeed = gravity * Time.deltaTime;
        moveSpeed = originSpeed;
    }

    public void Initialize()
    {
        StartCoroutine(URoutine());
        StartCoroutine(FRoutine());
        ChangeControll(true);
    }

    public void ChangeControll(bool controll)
    {
        haveControll = controll;
    }

    IEnumerator URoutine()
    {
        while (true)
        {
            if (haveControll)
            {
                Move();
            }
            yield return null;
        }
    }

    IEnumerator FRoutine()
    {
        while (true)
        {
            if (haveControll)
            {
                controller.Move(fowarVec * inputDir.z * moveSpeed * Time.deltaTime);
                controller.Move(rightVec * inputDir.x * moveSpeed * Time.deltaTime);
            }
            controller.Move(Vector3.up * ySpeed);
            yield return new WaitForFixedUpdate();
        }
    }

    private void Move()
    {
        fowarVec = new Vector3(Vector3.forward.x, 0, Vector3.forward.z);
        rightVec = new Vector3(Vector3.right.x, 0, Vector3.right.z);

        if (inputDir.magnitude == 0)
        {
            curSpeed = Mathf.Lerp(curSpeed, 0, 0.9f);
            anim.SetFloat("IsMoved", curSpeed);
            return;
        }

        if (inputDir != null)
        {
            curSpeed = Mathf.Lerp(curSpeed, moveSpeed, 0.1f);
        }

        if (isPulling)
        {
            // 현재 Player 오브젝트와 잡아당기려는 Player 오브젝트 사이의 방향 Vector를 계산 후 차이만큼 거리를 구한다.
            Vector3 directionToTarget = (transform.position - pullingPlayer.transform.position).normalized;

            // 잡아당기려는 Player 오브젝트의 CharacterController 컴포넌트를 이용하여, 계산된 방향과 pullForce만큼 힘을 가해서 Player를 잡아당긴다.
            controller.Move(directionToTarget * -pullForce);

            // 잡아당기는 Player가 잡히는 Player를 바라보도록 회전시킨다.
            transform.LookAt(pullingPlayer.transform.position, Vector3.up);
        }

        // 회전
        transform.LookAt(transform.position + Vector3.forward * inputDir.z + Vector3.right * inputDir.x);
        anim.SetFloat("IsMoved", curSpeed);

    }

    private void OnMove(InputValue value)
    {
        inputDir.x = value.Get<Vector2>().x;
        inputDir.z = value.Get<Vector2>().y;
    }



    public IEnumerator speedChangerRoutine()
    {
        if (isSpeedUp)
        {
            Debug.Log("스피드업 코루틴 시작");
            moveSpeed = moveSpeed * 1.5f; // speedIncrease만큼 배속을 곱적용 한다.
            yield return new WaitForSeconds(3); // speedIncreaseDuration시간만큼 지속한다.
            Debug.Log("스피드업 코루틴 종료");
            moveSpeed = originSpeed; //speedIncrease로 나누어 원본으로 돌아간다.
            isSpeedUp = false;
        }

        if (isSpeedDown)
        {
            Debug.Log("스피드다운 코루틴 시작");
            moveSpeed = moveSpeed * 0.5f; // speedIncrease만큼 배속을 나눔 적용한다.
            yield return new WaitForSeconds(3); // speedIncreaseDuration시간만큼 지속한다.
            Debug.Log("스피드다운 코루틴 종료");
            moveSpeed = originSpeed; //speedIncrease로 곱적용하여 원본으로 돌아간다.
            isSpeedDown = false;
        }

        if (isStun)
        {
            Debug.Log("스턴 코루틴 시작");
            // moveSpeed = 0; // 스턴하여 0으로 이동속도가 고정된다.
            haveControll = false;
            anim.SetBool("IsStun", true);
            yield return new WaitForSeconds(2); // speedIncreaseDuration시간만큼 지속한다.
            anim.SetBool("IsStun", false);
            Debug.Log("스턴 코루틴 종료");
            // moveSpeed = originSpeed; //스턴이 풀리면 10으로 이동속도가 돌아온다.
            haveControll = true;
            isStun = false;
        }

    }

    [PunRPC]
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;

    }

    [PunRPC]
    public void OnSpeedUp(int viewID) // 스피드업
    {
        if (photonView.ViewID != viewID)
            return;
        isSpeedUp = true;
        startSpeedChangeTIme = Time.time;
        StartCoroutine(speedChangerRoutine());
    }

    [PunRPC]
    public void OnSpeedDown(int viewID) // 스피드다운
    {
        if (photonView.ViewID != viewID)
            return;
        isSpeedDown = true;
        startSpeedChangeTIme = Time.time;
        StartCoroutine(speedChangerRoutine());
    }

    [PunRPC]
    public void OnStun(int viewID) // 스턴
    {
        if (photonView.ViewID != viewID)
            return;
        isStun = true;
        startSpeedChangeTIme = Time.time;
        StartCoroutine(speedChangerRoutine());
    }

    [PunRPC]
    public void mePullingStart(int ViewID) // 내가 당겨지면 호출되는 함수(moveDir는 나를 당기는 다른 Player 위치)
    {
        pullingPlayer = PhotonView.Find(ViewID).gameObject;
        isPulling = true;
    }

    [PunRPC]
    public void mePullingFinish() // 내가 당긴후 호출되는 함수
    {
        pullingPlayer = null;
        isPulling = false;
    }

    [PunRPC]
    public void mePushing(Vector3 moveDir) // 내가 밀린다면 호출되는 함수(moveDir는 나를 미는 다른 Player 위치)
    {
        controller.Move(moveDir);
    }

}