using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
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

    [SerializeField] private float pullForce; // ��ƴ��� ��

    private Animator anim;
    private CharacterController controller;
    private Rigidbody rigid;
    private PlayerInput playerInput;

    [SerializeField] GameObject pullingPlayer;
    [SerializeField] private Transform lookRotation;


    private Vector3 inputDir;
    Vector3 fowarVec;
    Vector3 rightVec;
    Vector3 v;


    private float ySpeed;

    private bool isPulling = false; // PlayerPuller Ŭ�����κ��� ���޹��� ��
    // private bool isPushing = false; // PlayerPuller Ŭ�����κ��� ���޹��� ��

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
        if (!photonView.IsMine)
            return;
        StartCoroutine(URoutine());
        StartCoroutine(FRoutine());
        StartCoroutine(AnimMove());
        
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
                if(!isPulling)
                {
                    controller.Move(fowarVec * inputDir.z * moveSpeed * Time.deltaTime);
                    controller.Move(rightVec * inputDir.x * moveSpeed * Time.deltaTime);
                }
                else
                {

                    // ��ƴ��� Player�� ������ Player�� �ٶ󺸵��� ȸ����Ų��.
                    transform.LookAt(pullingPlayer.transform.position, Vector3.up);

                    controller.Move(fowarVec * ((inputDir.z * moveSpeed) + v.z) * Time.deltaTime);
                    Debug.LogError($" FV : {fowarVec}) iD : {inputDir}) v : {v}) mS : {moveSpeed})"  );
                    controller.Move(rightVec * ((inputDir.x * moveSpeed) + v.x) * Time.deltaTime);
                }
                
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
            // anim.SetFloat("IsMoved", curSpeed);
            return;
        }

        if (inputDir != null)
        {
            curSpeed = Mathf.Lerp(curSpeed, moveSpeed, 0.1f);
        }

        // ȸ��
        transform.LookAt(transform.position + Vector3.forward * inputDir.z + Vector3.right * inputDir.x);
    }

    private IEnumerator AnimMove()
    {
        
        while (!anim.GetBool("IsDied"))
        {
            photonView.RPC("AnimChangeMove", RpcTarget.AllViaServer, curSpeed);
            yield return new WaitForSeconds(0.1f);
        }
            
    }

    [PunRPC]
    public void AnimChangeMove(float getSpeed)
    {
        anim.SetFloat("IsMoved", getSpeed);
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
           
            moveSpeed = moveSpeed * 1.5f; // speedIncrease��ŭ ����� ������ �Ѵ�.
            yield return new WaitForSeconds(3); // speedIncreaseDuration�ð���ŭ �����Ѵ�.
            
            moveSpeed = originSpeed; //speedIncrease�� ������ �������� ���ư���.
            isSpeedUp = false;
        }

        if (isSpeedDown)
        {
            
            moveSpeed = moveSpeed * 0.5f; // speedIncrease��ŭ ����� ���� �����Ѵ�.
            yield return new WaitForSeconds(3); // speedIncreaseDuration�ð���ŭ �����Ѵ�.
            
            moveSpeed = originSpeed; //speedIncrease�� �������Ͽ� �������� ���ư���.
            isSpeedDown = false;
        }

        if (isStun)
        {
            
            // moveSpeed = 0; // �����Ͽ� 0���� �̵��ӵ��� �����ȴ�.
            haveControll = false;
            anim.SetTrigger("IsStun");
            yield return new WaitForSeconds(2); // speedIncreaseDuration�ð���ŭ �����Ѵ�.
            
            // moveSpeed = originSpeed; //������ Ǯ���� 10���� �̵��ӵ��� ���ƿ´�.
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
        isSpeedDown = true;
        startSpeedChangeTIme = Time.time;
        StartCoroutine(speedChangerRoutine());
    }
    public void RequestSpeedDown(int viewId)
    {
        photonView.RPC("OnSpeedDown", RpcTarget.AllViaServer, viewId);
    }

    [PunRPC]
    public void OnStun(int viewID) // ����
    {
        isStun = true;
        startSpeedChangeTIme = Time.time;
        StartCoroutine(speedChangerRoutine());
    }
    public void RequestStun(int viewId)
    {
        photonView.RPC("OnStun", RpcTarget.AllViaServer, viewId);
    }

    public void RequestPullStart(int viewId)
    {
        photonView.RPC("mePullingStart", RpcTarget.AllViaServer, viewId);
    }

    [PunRPC]
    public void mePullingStart(int ViewID) // ���� ������� ȣ��Ǵ� �Լ�(moveDir�� ���� ���� �ٸ� Player ��ġ)
    {
        //Debug.LogError($"mover.isPulling -> true ȣ�� , viewId : {ViewID}");
        PhotonView view = PhotonView.Find(ViewID);
        //Debug.LogError($"{view.gameObject.name}");
        pullingPlayer = view.gameObject;
        //Debug.LogError($"{pullingPlayer.name}");

        // ���� Player ������Ʈ�� ��ƴ����� Player ������Ʈ ������ ���� Vector�� ��� �� ���̸�ŭ �Ÿ��� ���Ѵ�.
        Vector3 directionToTarget = (transform.position - pullingPlayer.transform.position).normalized;

        // ��ƴ����� Player ������Ʈ�� CharacterController ������Ʈ�� �̿��Ͽ�, ���� ����� pullForce��ŭ ���� ���ؼ� Player�� ��ƴ���.
        v = (directionToTarget * -pullForce);
        isPulling = true;
    }

    [PunRPC]
    public void mePullingFinish() // ���� ����� ȣ��Ǵ� �Լ�
    {
        //Debug.LogError("mover.isPulling -> false ȣ��");
        //Debug.LogError($"{PhotonNetwork.LocalPlayer.NickName}");
        pullingPlayer = null;
        isPulling = false;
        v = Vector3.zero;
    }

    [PunRPC]
    public void mePushing(Vector3 moveDir) // ���� �и��ٸ� ȣ��Ǵ� �Լ�(moveDir�� ���� �̴� �ٸ� Player ��ġ)
    {
        //Debug.LogError($"Pushed {moveDir}, {controller.velocity}");
        StartCoroutine(PushMoveRoutine(moveDir));
    }

    IEnumerator PushMoveRoutine(Vector3 moveDir)
    {
        for(int i = 0; i < 20; i++)
        {
            controller.Move(moveDir * 0.05f);
            yield return new WaitForFixedUpdate();
        }
    }

}