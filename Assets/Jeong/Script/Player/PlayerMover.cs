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
    [SerializeField] public float moveSpeed; // �̵��ӵ�
    [SerializeField] private float maxSpeed; // �ִ� �̵��ӵ�

    [SerializeField] public float rotateSpeed; // ȸ���ӵ�

    [SerializeField] private bool keepFacingTarget = false; // ������ �÷��̾ ����ؼ� �ٶ󺸵��� ���� ����

    private float curSpeed = 0f;

    private GameObject targetPlayer; // �������� �÷��̾ �����ϴ� ����

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

        // ȸ��
        if (moveDir != Vector3.zero)
        {
            if (keepFacingTarget && targetPlayer != null)
            {
                // ������ �÷��̾ �ٶ󺸵��� ȸ��
                transform.LookAt(targetPlayer.transform.position, Vector3.up);
            }
            else
            {
                // �̵� �������� ȸ��
                transform.forward = Vector3.Lerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
            }
        }

        // �ִ�ӷ�
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