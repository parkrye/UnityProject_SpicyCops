using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [SerializeField] private float curAggro;
    [SerializeField] private float maxAggro;

    private enum PlayerState { Idle, Attack, Hit, Targetting, Slow, Strun, Dead, Size } // ���, ����, Ÿ��,  �ǰ�, ��ȭ, ����, ����

    private PlayerState curState;
    private PlayerState lastState;

    
    private void AggroGauge()
    {
        // Player ��׷� ��ġ ����
        // �ΰ��ӸŴ����� ���� ��׷μ�ġ�� ������ �߽ſ�û�Ѵ�.
        // �������� �����ϸ� ��׷μ�ġ�� �ΰ��ӸŴ����� ���� �ٸ� Player�� Enemy���� ��׷� ��ġ�� �߽��Ѵ�.
        // �ΰ��� �Ŵ����� ���� �ٸ�Player�� ��׷μ�ġ�� �޾ƿ´�.
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

            case PlayerState.Targetting:
                DoTargetting();
                break;

            case PlayerState.Dead:
                DoDead();
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
        }
    }


    private void DoIdle()
    {
        // ������, �ٸ� ���� ����
        // Attack, Hit, Targetting, Sturn, Caught ��ȯ
    }


    private void DoAttack()
    {
        // ���ݻ���
        // Idle ��ȯ
        // ������ ���
    }

    private void DoHit()
    {
        // �ǰݻ���, �ٸ� ���� ����
        // Idle, Slow, Sturn, Targetting ��ȯ 
        // ������ �ǰ� 
    }

    private void DoTargetting()
    {
        // ��׷μ�ġ�� ���� ���� ����
        // Idle, Caught ��ȯ
        // ������ �ǰ�
    }


    private void DoSlow()
    {
        // Hit�� ���� ��ȭ����
        // Idle, Targetting, Caught ��ȯ
        // ������ �ǰ�
    }

    private void DoSturn()
    {
        // Hit�� ���� ��������
        // Idle, Targetting, Caught ��ȯ
        // ������ �ǰ�
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
