using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMover : MonoBehaviourPun
{
    [SerializeField] public float curSpeed = 0f;
    [SerializeField] public float moveSpeed; // �̵��ӵ�
    [SerializeField] public float rotateSpeed; // ȸ���ӵ�


    [SerializeField] public float speedIncrease; // �̵��ӵ� ������
    [SerializeField] public float speedChangeDuration; // �̵��ӵ� ���� �ð�

    [SerializeField] private float startSpeedChangeTIme; // �̵��ӵ� ���� ���� �ð�


    private Animator anim;
    private CharacterController controller;
    private Rigidbody rigid;
    private PlayerInput playerInput;

    [SerializeField] private Transform lookRotation;


    private Vector3 inputDir;
    private float ySpeed;

    private bool isPulling = false; // PlayerPuller Ŭ�����κ��� ���޹��� ��

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

        // ȸ��

        transform.LookAt(transform.position + Vector3.forward * inputDir.z + Vector3.right * inputDir.x);

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
            Debug.Log("���ǵ�� �ڷ�ƾ ����");
            moveSpeed = moveSpeed * speedIncrease; // speedIncrease��ŭ ����� ������ �Ѵ�.
            yield return new WaitForSeconds(speedChangeDuration); // speedIncreaseDuration�ð���ŭ �����Ѵ�.
            Debug.Log("���ǵ�� �ڷ�ƾ ����");
            moveSpeed = moveSpeed / speedIncrease; //speedIncrease�� ������ �������� ���ư���.
            isSpeedUp = false;
        }

        if (isSpeedDown)
        {
            Debug.Log("���ǵ�ٿ� �ڷ�ƾ ����");
            moveSpeed = moveSpeed / speedIncrease; // speedIncrease��ŭ ����� ���� �����Ѵ�.
            yield return new WaitForSeconds(speedChangeDuration); // speedIncreaseDuration�ð���ŭ �����Ѵ�.
            Debug.Log("���ǵ�ٿ� �ڷ�ƾ ����");
            moveSpeed = moveSpeed * speedIncrease; //speedIncrease�� �������Ͽ� �������� ���ư���.
            isSpeedDown = false;
        }

        if (isSturn)
        {
            Debug.Log("���� �ڷ�ƾ ����");
            moveSpeed = 0; // �����Ͽ� 0���� �̵��ӵ��� �����ȴ�.
            yield return new WaitForSeconds(speedChangeDuration); // speedIncreaseDuration�ð���ŭ �����Ѵ�.
            Debug.Log("���� �ڷ�ƾ ����");
            moveSpeed = 10; //������ Ǯ���� 10���� �̵��ӵ��� ���ƿ´�.
            isSturn = false;
        }
    }

    public void OnSpeedUp() // ���ǵ��
    {
        isSpeedUp = true;
        startSpeedChangeTIme = Time.time;
        StartCoroutine(speedChangerRoutine());
    }

    public void OnSpeedDown() // ���ǵ�ٿ�
    {
        isSpeedDown = true;
        startSpeedChangeTIme = Time.time;
        StartCoroutine(speedChangerRoutine());
    }

    public void OnSturn() // ����
    {
        isSturn = true;
        startSpeedChangeTIme = Time.time;
        StartCoroutine(speedChangerRoutine());
    }
}