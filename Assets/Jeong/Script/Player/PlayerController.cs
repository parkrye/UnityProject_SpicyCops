using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [SerializeField] private float curAggro;
    [SerializeField] private float maxAggro;

    private enum PlayerState { Idle, Attack, Hit, Slow, Strun, Dead, Size } // ���, ����, �ǰ�, ��ȭ, ����, ����

    private PlayerState curState;

    private void Start()
    {
        curState = PlayerState.Idle;
    }

    private void Update()
    {
        switch (curState)
        {
            case PlayerState.Idle:
                DoIdle();
                break;

            case PlayerState.Attack:
                DoAttack();
                break;

            case PlayerState.Hit:
                DoHit();
                break;

            case PlayerState.Slow:
                DoSlow();
                break;

            case PlayerState.Strun:
                DoSturn();
                break;

            case PlayerState.Dead:
                DoDead();
                break;
        }
    }

    private void AggroGauge()
    {
        // Player ��׷� ��ġ ����
        // �ΰ��ӸŴ����� ���� ��׷μ�ġ�� ������ �߽ſ�û�Ѵ�.
        // �������� �����ϸ� ��׷μ�ġ�� �ΰ��ӸŴ����� ���� �ٸ� Player�� Enemy���� ��׷� ��ġ�� �߽��Ѵ�.
        // �ΰ��� �Ŵ����� ���� �ٸ�Player�� ��׷μ�ġ�� �޾ƿ´�.
    }

    private void DoIdle()
    {
        // ������, �ٸ� ���� ����
        // Attack, Hit,  Sturn, Dead ��ȯ

        // ���� ������ ����� DoAttack ������ȯ
        // �ǰ� ���ҽ� DoHit ������ȯ
        // Enemy���� ������ Dead ������ȯ
    }


    private void DoAttack()
    {
        // ���ݻ���
        // Idle ��ȯ
        // ������ ���

        // ���� ������ ��� �� DoIdle ������ȯ
        // Enemy���� ������ Dead ������ȯ
    }

    private void DoHit()
    {
        // �ǰݻ���, �ٸ� ���� ����
        // Slow, Sturn ��ȯ 

        // ��ȭ ������ �ǰݽ� Slow ������ȯ
        // ���� ������ �ǰݽ� Strun ������ȯ
        // Enemy���� ������ Dead ������ȯ
    }

    private void DoSlow()
    {
        // ������ �ǰ� ��ȭ
        // Idle, Dead ��ȯ

        // ��ȭ �� �����ð� ���� Idle ������ȯ
        // Enemy���� ������ Dead ������ȯ
    }

    private void DoSturn()
    {
        // ������ �ǰ� ����
        // Idle, Dead ��ȯ

        // ���� �� �����ð� ���� Idle ������ȯ
        // Enemy���� ������ Dead ������ȯ
    }

    private void DoDead()
    {
        // Enemy���� ����

        // Enmey�� Player���� ������ DoCaught�Լ��� ������ ȣ���Ѵ�.

        // �״� �ִϸ��̼� ���
        // ��׷� ��ġ 0���� ����
        // �Էµ� �����͸� 0���� �ʱ�ȭ �� �Է� ����
    }
}
