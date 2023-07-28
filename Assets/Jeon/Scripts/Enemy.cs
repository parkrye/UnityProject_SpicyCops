using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] GameObject[] players;

    Dictionary<string, int> playerState = new Dictionary<string, int>();

    [SerializeField] float moveSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float targetFindTime;

    public enum EnemyState { Idle, Follow, Angry, Berserker }

    private EnemyState curState = EnemyState.Follow;

    private int curTime;            // ����ð��� ���ӳ����� �ð��� �����Ų�� �� : ó�� �ð��� 180��

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void Update()
    {
        switch (curState)
        {
            case EnemyState.Idle:
                DoIdle();
                break;
            case EnemyState.Follow:
                DoFollow();
                break;
            case EnemyState.Angry:
                DoAngry();
                break;
            case EnemyState.Berserker:
                DoBerserker();
                break;
        }
    }

    private void DoIdle()
    {
        // ������ ������ �� ��� ���ʵ��� ������ �ִ´�
        if (curTime >= 180)
        {

        }
        else if (curTime == 175)
        {
            // �ִϸ��̼� �������ֱ�
            curState = EnemyState.Follow;
        }
    }
    private void DoFollow()
    {
        // Player�߿� ��׷μ�ġ�� ���� ���� player�� �i�´�
        IEnumerator follow = FollowMoving();
        if (curTime > 120)
        {
            StartCoroutine(follow);
        }
        else if (curTime == 70)
        {
            // �ִϸ��̼� �������ֱ�
            StopCoroutine(follow);
            curState = EnemyState.Angry;

        }
    }

    private void DoAngry()
    {
        // �̵��ӵ��� ������Ű�� ��׷μ�ġ�� ���� ���� player�� �i�´�
        // IEnumerator angry;
        if (curTime > 30)
        {

        }
        else
        {
            // �ִϸ��̼� �������ֱ�
            curState = EnemyState.Berserker;
        }
    }

    private void DoBerserker()
    {
        // �̵��ӵ��� ���� �� ������Ű�� ��׷μ�ġ�� ���� ���� player�� �i�´�

        // �ִϸ��̼� �������ֱ�


    }

    IEnumerator FollowMoving()
    {
        while (true)
        {
            if (moveSpeed >= maxSpeed)
            {

            }
        }
    }

    /*IEnumerator AngryMoving()
    {

    }*/
}
