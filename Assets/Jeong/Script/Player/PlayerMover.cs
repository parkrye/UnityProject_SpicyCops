using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMover : MonoBehaviourPun
{
    [SerializeField] public float curSpeed = 0f;
    [SerializeField] public float moveSpeed; // 이동속도
    [SerializeField] public float rotateSpeed; // 회전속도


    [SerializeField] public float speedIncrease; // 이동속도 증가율
    [SerializeField] public float speedChangeDuration; // 이동속도 변경 시간

    [SerializeField] private float startSpeedChangeTIme; // 이동속도 증가 시작 시간


    private Animator anim;
    private CharacterController controller;
    private Rigidbody rigid;
    private PlayerInput playerInput;

    [SerializeField] private Transform lookRotation;


    private Vector3 inputDir;
    private float ySpeed;

    private bool isPulling = false; // PlayerPuller 클래스로부터 전달받은 값

    private bool isSpeedUp = false;
    private bool isSpeedDown = false;
    private bool isSturn = false;

    private void Start()
    {

        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        rigid = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        if (!photonView.IsMine)
            Destroy(playerInput);

        lookRotation.position = new Vector3(0, 0, 0.2f);
    }
    
    private void FixedUpdate()
    {
        
        Move();
        //lookRotation.localPosition = (new Vector3(inputDir.x + transform.position.x, 0, inputDir.z + transform.position.z) + transform.forward * 0.2f);

    }

    private void Move()
    {
        if (inputDir.magnitude == 0)
        {
            curSpeed = Mathf.Lerp(curSpeed, 0, 0.9f);
            anim.SetFloat("IsMoved", curSpeed);
            return;
        }

        Vector3 fowarVec = new Vector3(Vector3.forward.x, 0, Vector3.forward.z);
        Vector3 rightVec = new Vector3(Vector3.right.x, 0, Vector3.right.z);


        ySpeed += Physics.gravity.y * Time.deltaTime;

        controller.Move(Vector3.up * ySpeed);

        if (inputDir != null)
        {
            curSpeed = Mathf.Lerp(curSpeed, moveSpeed, 0.1f);
        }

        // 회전

        transform.LookAt(transform.position + Vector3.forward * inputDir.z + Vector3.right * inputDir.x);

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

        controller.Move(fowarVec * inputDir.z * moveSpeed * Time.deltaTime);
        controller.Move(rightVec * inputDir.x * moveSpeed * Time.deltaTime);

        anim.SetFloat("IsMoved", curSpeed);
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
            moveSpeed = moveSpeed * speedIncrease; // speedIncrease만큼 배속을 곱적용 한다.
            yield return new WaitForSeconds(speedChangeDuration); // speedIncreaseDuration시간만큼 지속한다.
            Debug.Log("스피드업 코루틴 종료");
            moveSpeed = moveSpeed / speedIncrease; //speedIncrease로 나누어 원본으로 돌아간다.
            isSpeedUp = false;
        }

        if (isSpeedDown)
        {
            Debug.Log("스피드다운 코루틴 시작");
            moveSpeed = moveSpeed / speedIncrease; // speedIncrease만큼 배속을 나눔 적용한다.
            yield return new WaitForSeconds(speedChangeDuration); // speedIncreaseDuration시간만큼 지속한다.
            Debug.Log("스피드다운 코루틴 종료");
            moveSpeed = moveSpeed * speedIncrease; //speedIncrease로 곱적용하여 원본으로 돌아간다.
            isSpeedDown = false;
        }

        if (isSturn)
        {
            Debug.Log("스턴 코루틴 시작");
            moveSpeed = 0; // 스턴하여 0으로 이동속도가 고정된다.
            yield return new WaitForSeconds(speedChangeDuration); // speedIncreaseDuration시간만큼 지속한다.
            Debug.Log("스턴 코루틴 종료");
            moveSpeed = 10; //스턴이 풀리면 10으로 이동속도가 돌아온다.
            isSturn = false;
        }
    }

    public void OnSpeedUp() // 스피드업
    {
        isSpeedUp = true;
        startSpeedChangeTIme = Time.time;
        StartCoroutine(speedChangerRoutine());
    }

    public void OnSpeedDown() // 스피드다운
    {
        isSpeedDown = true;
        startSpeedChangeTIme = Time.time;
        StartCoroutine(speedChangerRoutine());
    }

    public void OnSturn() // 스턴
    {
        isSturn = true;
        startSpeedChangeTIme = Time.time;
        StartCoroutine(speedChangerRoutine());
    }
}