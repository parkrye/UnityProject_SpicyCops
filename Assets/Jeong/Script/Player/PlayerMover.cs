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
    [SerializeField] private float moveSpeed; // �̵��ӵ�
    [SerializeField] private float maxSpeed; // �ִ� �̵��ӵ�

    [SerializeField] private float roateSpeed; // ȸ���ӵ�

    [SerializeField] private float pullForce; // ��ƴ��� ��
    [SerializeField] private float pullRange; // ��ƴ��� ����

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

        // isPulling�� true�� ����ؼ� Ÿ���� ã�´�.
        if (isPulling)
        {
            FindTargetPlayer();
        }


    }

    private void Move()
    {
        Vector3 moveDir = new Vector3(-inputDir.x, 0, -inputDir.y);
        rigid.velocity = moveDir * moveSpeed * Time.deltaTime;

        // ȸ��
        if (moveDir != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, moveDir, roateSpeed * Time.deltaTime);
        }

        // �ִ�ӷ�
        if (rigid.velocity.sqrMagnitude > maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }

        // Ÿ���� ã���� ��ƴ���.
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
        // space�� ������ 
        if (value.isPressed)
        {
            isPulling = true;
        }

        else
        {
            isPulling = false;
            targetPlayer = null; // ��� ����
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