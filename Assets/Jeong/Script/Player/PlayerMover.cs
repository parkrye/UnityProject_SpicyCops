using Photon.Pun;
using Photon.Pun.Demo.Procedural;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerMover : MonoBehaviourPun
{
    [SerializeField] public float moveSpeed; // 이동속도
    [SerializeField] private float maxSpeed; // 최대 이동속도

    [SerializeField] public float rotateSpeed; // 회전속도

    private float curSpeed = 0f;

    private Animator anim;
    private Rigidbody rigid;
    private PlayerInput playerInput;

    private Vector2 inputDir;
    private float ySpeed;

    private bool isPulling = false; // PlayerPuller 클래스로부터 전달받은 값

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        if (!photonView.IsMine)
            Destroy(playerInput);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);

        ySpeed += Physics.gravity.y * Time.deltaTime;
        rigid.velocity = moveDir * moveSpeed * Time.deltaTime + Vector3.up * rigid.velocity.y;

        curSpeed = rigid.velocity.magnitude;

        // 회전
        if (moveDir != Vector3.zero)
        {
            // 이동 방향으로 회전 (잡기상태가 아닐떄)
            if(!isPulling)
            transform.forward = Vector3.Lerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
        }

        // 최대속력
        if (rigid.velocity.sqrMagnitude > maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }

        anim.SetFloat("IsMoved", curSpeed);
    }

    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

  

}