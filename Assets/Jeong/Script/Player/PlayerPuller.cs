using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPuller : MonoBehaviour
{
    [SerializeField] private bool debug;
    [SerializeField] private float pullForce; // ��ƴ��� ��
    [SerializeField] private float pullRange; // ��ƴ��� ����
    [SerializeField] private PlayerMover mover;

    [SerializeField] private bool isPulling = false;

    private GameObject targetPlayer;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        PullTarget();
        // isPulling�� true�� ����ؼ� Ÿ���� ã�´�.
        if (isPulling)
        {
            FindTargetPlayer();
        }
    }

    private void OnPull(InputValue value)
    {
        // xŰ�� ������ 
        if (value.isPressed)
        {
            isPulling = true;
            anim.SetBool("IsPulled", true);
        }

        else
        {
            isPulling = false;
            anim.SetBool("IsPulled", false);
            targetPlayer = null; // ��� ����
        }
    }

    private void PullTarget()
    {
        // Ÿ���� ã���� ��ƴ���.
        if (isPulling && targetPlayer != null)
        {
            Pull(targetPlayer);
        }
    }

    private void FindTargetPlayer() // ��ƴ�� Player Ž��
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRange);

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

    // ��ƴ��� 
    private void Pull(GameObject player)
    {
        // ���� Player ������Ʈ�� ��ƴ����� Player ������Ʈ ������ ���� Vector�� ��� �� ���̸�ŭ �Ÿ��� ���Ѵ�.
        Vector3 directionToTarget = (player.transform.position - transform.position).normalized;

        // ��ƴ����� Player ������Ʈ�� Rigidbody ������Ʈ�� �̿��Ͽ�, ���� ����� pullForce��ŭ ���� ���ؼ� Player�� ��ƴ���.
        player.GetComponent<Rigidbody>().AddForce(directionToTarget * -pullForce, ForceMode.Force);

        // ��ƴ��� Player�� ������ Player�� �ٶ󺸵��� ȸ����Ų��.
        player.transform.LookAt(transform.position, Vector3.up);

        // ��ƴ��� Player�� �ٶ󺸵��� ȸ����Ų��.
        transform.LookAt(player.transform.position, Vector3.up);

        // ��ƴ��� Player�� ������ Player�� �ӵ��� ��������.
        float slowDownFactor = 0.5f; // ���� ������ �ӵ��� ���ҽ�Ű�� ���� ����
        player.GetComponent<PlayerMover>().SetMoveSpeed(player.GetComponent<PlayerMover>().moveSpeed * slowDownFactor);

        // PlayerMover�� targetPlayer�� �����Ѵ�.
        player.GetComponent<PlayerMover>().SetTargetPlayer(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (!debug)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pullRange);
    }
}
