using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPusher : MonoBehaviour
{/*
    [SerializeField] private bool debug;
    [SerializeField] private float pushForce; // 미는 힘
    [SerializeField] private float pushRange; // 밀 수 있는 범위
    [SerializeField] private PlayerMover mover;

    [SerializeField] private bool isPushing = false;

    private GameObject targetPlayer;
    private Animator anim;

    private Coroutine pushRoutine;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        PushTarget();
        // isPushing이 true면 계속해서 타겟을 찾는다.
        if (isPushing)
        {
            FindTargetPlayer();
        }
    }

    private void OnPush(InputValue value)
    {
        // z키를 누르면 
        if (value.isPressed)
        PushTarget();
    }

    private void PushTarget()
    {
        // 타겟을 찾으면 민다.
        if (isPushing && targetPlayer != null)
        {
            pushRoutine = StartCoroutine(PushRoutine);
        }
    }

    IEnumerator PushRoutine()
    {
        while (true)
        {
            Push(targetPlayer);
            anim.SetTrigger("IsPushed");
            isPushing = true;
            yield return new WaitForSeconds(10f);
            isPushing = false;
        }
    }

    private void FindTargetPlayer() // 밀쳐버릴 Player 탐색
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pushRange);

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

    // 밀기
    private void Push(GameObject player)
    {
        // 현재 Player 오브젝트와 미려는 Player 오브젝트 사이의 방향 Vector를 계산 후 차이만큼 거리를 구한다.
        Vector3 directionToTarget = (player.transform.position - transform.position).normalized;

        // 미려는 Player 오브젝트의 Rigidbody 컴포넌트를 이용하여, 계산된 방향과 pullForce만큼 힘을 가해서 Player를 민다.
        player.GetComponent<Rigidbody>().AddForce(directionToTarget * pushForce, ForceMode.Impulse);
        
        // ToDo
        // 미려는 Player는 밀리는 Player를 바라봐야한다.
    }


    private void OnDrawGizmosSelected()
    {
        if (!debug)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pushRange);
    }*/
}
