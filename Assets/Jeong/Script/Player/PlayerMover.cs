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

    [SerializeField] private bool debug;
    [SerializeField] private float moveSpeed; // 이동속도
    [SerializeField] private float maxSpeed; // 최대 이동속도

    [SerializeField] private float roateSpeed; // 회전속도

    [SerializeField] private float pullForce; // 잡아당기는 힘
    [SerializeField] private float pullRange; // 잡아당기는 범위

    private Rigidbody rigid;
    private Vector2 inputDir;

    private bool isPulling = false;

    private GameObject targetPlayer;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();

        // isPulling이 true면 계속해서 타겟을 찾는다.
        if (isPulling)
        {
            FindTargetPlayer();
        }


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

        // 타겟을 찾으면 잡아당긴다.
        if (isPulling && targetPlayer != null)
        {
            Pull(targetPlayer);
        }

    }

    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
    }


    private void OnPull(InputValue value)
    {
        // space를 누르면 
        if (value.isPressed)
        {
            isPulling = true;
        }

        else
        {
            isPulling = false;
            targetPlayer = null; // 잡기 해제
        }
    }

    private void FindTargetPlayer() // 잡아당길 Player 탐색
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRange);

        // 가장 가까운 Player와의 거리를 최대값으로 초기화하고, Player의 GameObject를 null로 초기화한다.
        float closestDistance = float.MaxValue;
        GameObject closestPlayer = null;
        
        // 범위안의 Collider를 모두 확인하는 작업
        foreach (Collider collider in colliders)
        {
            // Player 태그 탐색
            if (collider.gameObject != gameObject && collider.gameObject.CompareTag("Player"))
            {
                // 현재 Player와 다른 Player 사이의 거리 계산
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                // 거리가 더 가까운 Player를 찾으면
                if (distance < closestDistance)
                {
                    // 최대값으로 초기화했던 가장 가까운거리를 새로찾은 거리로 갱신하고
                    closestDistance = distance;
                    // 마찬가지로 null이 들어있던 Player의 Gameobject도 새로 갱신한다.
                    closestPlayer = collider.gameObject;
                }
            }
        }
        // 가장 가까운 Player을 탐색하고 targetPlayer 변수에 갱신한다.
        targetPlayer = closestPlayer;
    }

    // 잡아당기기 
    private void Pull(GameObject player)
    {
        // 현재 Player 오베직트와 잡아당기려는 Player 오브젝트 사이의 방향 Vector를 계산 후 차이만큼 거리를 구한다.
        Vector3 directionToTarget = (player.transform.position - transform.position).normalized;

        // 잡아당기려는 Player 오브젝트의 Rigidbody 컴포넌트를 이용하여, 계산된 방향과 pullForce만큼 힘을 가해서 Player를 잡아당긴다.
        player.GetComponent<Rigidbody>().AddForce(directionToTarget * -pullForce, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        if (!debug)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pullRange);
    }
}