using Photon.Pun.Demo.Procedural;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;

    [SerializeField] private float roateSpeed;

    private Rigidbody rigid;
    private Vector2 inputDir;
    

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveDir = new Vector3(-inputDir.x, 0, -inputDir.y);
        rigid.velocity = moveDir * moveSpeed * Time.deltaTime;
        
        // 회전
        if (moveDir != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, moveDir, roateSpeed * Time.deltaTime);
        }

        // 최대속력
        if (rigid.velocity.sqrMagnitude > maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }
    }

    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
    }    
}
