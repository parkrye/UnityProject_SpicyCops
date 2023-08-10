using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMover : MonoBehaviourPun
{
    [SerializeField] public float curSpeed = 0f;
    [SerializeField] public float originSpeed; // �̵��ӵ�
    [SerializeField] public float moveSpeed; // �̵��ӵ�
    [SerializeField] public float rotateSpeed; // ȸ���ӵ�


    [SerializeField] public float speedIncrease; // �̵��ӵ� ������
    [SerializeField] public float speedChangeDuration; // �̵��ӵ� ���� �ð�

    [SerializeField] private float startSpeedChangeTIme; // �̵��ӵ� ���� ���� �ð�
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

    private bool isPulling = false; // PlayerPuller Ŭ�����κ��� ���޹��� ��
    private bool isPushing = false; // PlayerPuller Ŭ�����κ��� ���޹��� ��

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

        // ȸ��

        transform.LookAt(transform.position + Vector3.forward * inputDir.z + Vector3.right * inputDir.x);
        anim.SetFloat("IsMoved", curSpeed);


        //if (fowarVec != Vector3.zero && rightVec != Vector3.zero)
        //{
        //    // �̵� �������� ȸ�� (�����°� �ƴҋ�)
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
            Debug.Log("���ǵ�� �ڷ�ƾ ����");
            moveSpeed = moveSpeed * 1.5f; // speedIncrease��ŭ ����� ������ �Ѵ�.
            yield return new WaitForSeconds(3); // speedIncreaseDuration�ð���ŭ �����Ѵ�.
            Debug.Log("���ǵ�� �ڷ�ƾ ����");
            moveSpeed = originSpeed; //speedIncrease�� ������ �������� ���ư���.
            isSpeedUp = false;
        }

        if (isSpeedDown)
        {
            Debug.Log("���ǵ�ٿ� �ڷ�ƾ ����");
            moveSpeed = moveSpeed * 0.5f; // speedIncrease��ŭ ����� ���� �����Ѵ�.
            yield return new WaitForSeconds(3); // speedIncreaseDuration�ð���ŭ �����Ѵ�.
            Debug.Log("���ǵ�ٿ� �ڷ�ƾ ����");
            moveSpeed = originSpeed; //speedIncrease�� �������Ͽ� �������� ���ư���.
            isSpeedDown = false;
        }

        if (isStun)
        {
            Debug.Log("���� �ڷ�ƾ ����");
            // moveSpeed = 0; // �����Ͽ� 0���� �̵��ӵ��� �����ȴ�.
            haveControll = false;
            anim.SetBool("IsStun", true);
            yield return new WaitForSeconds(2); // speedIncreaseDuration�ð���ŭ �����Ѵ�.
            anim.SetBool("IsStun", false);
            Debug.Log("���� �ڷ�ƾ ����");
            // moveSpeed = originSpeed; //������ Ǯ���� 10���� �̵��ӵ��� ���ƿ´�.
            haveControll = true;
            isStun = false;
        }
    }

    [PunRPC]
    public void OnSpeedUp(int viewID) // ���ǵ��
    {
        if (photonView.ViewID != viewID)
            return;
        isSpeedUp = true;
        startSpeedChangeTIme = Time.time;
        StartCoroutine(speedChangerRoutine());
    }

    [PunRPC]
    public void OnSpeedDown(int viewID) // ���ǵ�ٿ�
    {
        if (photonView.ViewID != viewID)
            return;
        isSpeedDown = true;
        startSpeedChangeTIme = Time.time;
        StartCoroutine(speedChangerRoutine());
    }

    [PunRPC]
    public void OnStun(int viewID) // ����
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