using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviourPun
{

    [SerializeField] public float curSpeed = 0f;
    [SerializeField] public float moveSpeed; // 이동속도
    [SerializeField] public float rotateSpeed; // 회전속도

    private Animator anim;
    private CharacterController controller;
    private Rigidbody rigid;
    private PlayerInput playerInput;

    [SerializeField] private Transform lookRotation;


    private Vector3 inputDir;
    private float ySpeed;

    private bool isPulling = false; // PlayerPuller 클래스로부터 전달받은 값

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
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
        Move();
        //lookRotation.localPosition = (new Vector3(inputDir.x + transform.position.x, 0, inputDir.z + transform.position.z) + transform.forward * 0.2f);

    }

    private void Move()
    {
        if ( inputDir.magnitude == 0)
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
}