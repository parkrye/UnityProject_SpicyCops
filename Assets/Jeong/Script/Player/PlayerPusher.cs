using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPusher : MonoBehaviour
{/*
    [SerializeField] private bool debug;
    [SerializeField] private float pushForce; // �̴� ��
    [SerializeField] private float pushRange; // �� �� �ִ� ����
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
        // isPushing�� true�� ����ؼ� Ÿ���� ã�´�.
        if (isPushing)
        {
            FindTargetPlayer();
        }
    }

    private void OnPush(InputValue value)
    {
        // zŰ�� ������ 
        if (value.isPressed)
        PushTarget();
    }

    private void PushTarget()
    {
        // Ÿ���� ã���� �δ�.
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

    private void FindTargetPlayer() // ���Ĺ��� Player Ž��
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pushRange);

        // ���� ����� Player���� �Ÿ��� �ִ밪���� �ʱ�ȭ�ϰ�, Player�� GameObject�� null�� �ʱ�ȭ�Ѵ�.
        float closestDistance = float.MaxValue;
        GameObject closestPlayer = null;

        // �������� Collider�� ��� Ȯ���ϴ� �۾�
        foreach (Collider collider in colliders)
        {
            // Player �±� Ž��
            if (collider.gameObject != gameObject && collider.gameObject.CompareTag("Player"))
            {
                // ���� Player�� �ٸ� Player ������ �Ÿ� ���
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                // �Ÿ��� �� ����� Player�� ã����
                if (distance < closestDistance)
                {
                    // �ִ밪���� �ʱ�ȭ�ߴ� ���� �����Ÿ��� ����ã�� �Ÿ��� �����ϰ�
                    closestDistance = distance;
                    // ���������� null�� ����ִ� Player�� Gameobject�� ���� �����Ѵ�.
                    closestPlayer = collider.gameObject;
                }
            }
        }
        // ���� ����� Player�� Ž���ϰ� targetPlayer ������ �����Ѵ�.
        targetPlayer = closestPlayer;
    }

    // �б�
    private void Push(GameObject player)
    {
        // ���� Player ������Ʈ�� �̷��� Player ������Ʈ ������ ���� Vector�� ��� �� ���̸�ŭ �Ÿ��� ���Ѵ�.
        Vector3 directionToTarget = (player.transform.position - transform.position).normalized;

        // �̷��� Player ������Ʈ�� Rigidbody ������Ʈ�� �̿��Ͽ�, ���� ����� pullForce��ŭ ���� ���ؼ� Player�� �δ�.
        player.GetComponent<Rigidbody>().AddForce(directionToTarget * pushForce, ForceMode.Impulse);
        
        // ToDo
        // �̷��� Player�� �и��� Player�� �ٶ�����Ѵ�.
    }


    private void OnDrawGizmosSelected()
    {
        if (!debug)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pushRange);
    }*/
}
