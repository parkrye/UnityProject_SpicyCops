using Photon.Pun;
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

    private Animator anim;
    private CharacterController controller;
    private Rigidbody rigid;
    private PlayerInput playerInput;

    [SerializeField] private Transform lookRotation;


    private Vector3 inputDir;
    Vector3 fowarVec;
    Vector3 rightVec;

    private float ySpeed;

    private bool isPulling = false; // PlayerPuller 클래스로부터 전달받은 값
    private bool isPushing = false; // PlayerPuller 클래스로부터 전달받은 값

    private bool isSpeedUp = false;
    private bool isSpeedDown = false;
    private bool isStun = false;
    bool haveControll = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        rigid = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

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
        while(true)
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

        // 회전

        transform.LookAt(transform.position + Vector3.forward * inputDir.z + Vector3.right * inputDir.x);
        anim.SetFloat("IsMoved", curSpeed);


        //if (fowarVec != Vector3.zero && rightVec != Vector3.zero)
        //{
        //    // 이동 방향으로 회전 (잡기상태가 아닐떄)
        //    if (!isPulling)
        //    {/*
        //        Quaternion lookRotation = Quaternion.Euler(rightVec.x, 0, fowarVec.z);
        //        lookRotation.eulerAngles = new Vector3();
        //        transform.rotation = lookRotation;*/

        //        transform.LookAt(lookRotation);
        //    }
        //}

    }

    private void OnMove(InputValue value)
    {
        inputDir.x = value.Get<Vector2>().x;
        inputDir.z = value.Get<Vector2>().y;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
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
    public void Annoy(int viewID)
    {
        if(photonView.ViewID != viewID)
        {
            if (isPulling)
            {

            }

            if (isPushing)
            {

            }
        }
       
    }
}