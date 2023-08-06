using Photon.Pun.Demo.Procedural;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] public float moveSpeed; // 이동속도
    [SerializeField] private float maxSpeed; // 최대 이동속도

    [SerializeField] public float rotateSpeed; // 회전속도

    [SerializeField] private bool keepFacingTarget = false; // 잡히는 플레이어를 계속해서 바라보도록 할지 여부

    private float curSpeed = 0f;

    private GameObject targetPlayer; // 잡혀지는 플레이어를 저장하는 변수

    private Animator anim;
    private Rigidbody rigid;
    private Vector2 inputDir;
    private float ySpeed;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
      
    }

    public void SetTargetPlayer(GameObject player)
    {
        targetPlayer = player;
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
            if (keepFacingTarget && targetPlayer != null)
            {
                // 잡히는 플레이어를 바라보도록 회전
                transform.LookAt(targetPlayer.transform.position, Vector3.up);
            }
            else
            {
                // 이동 방향으로 회전
                transform.forward = Vector3.Lerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
            }
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