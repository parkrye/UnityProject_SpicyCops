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

    private int curTime;            // 현재시간은 게임내에서 시간을 적용시킨다 약 : 처음 시간은 180초

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
        // 게임을 시작할 때 잠깐 몇초동안 가만히 있는다
        if (curTime >= 180)
        {

        }
        else if (curTime == 175)
        {
            // 애니메이션 실행해주기
            curState = EnemyState.Follow;
        }
    }
    private void DoFollow()
    {
        // Player중에 어그로수치가 가장 높은 player를 쫒는다
        IEnumerator follow = FollowMoving();
        if (curTime > 120)
        {
            StartCoroutine(follow);
        }
        else if (curTime == 70)
        {
            // 애니메이션 실행해주기
            StopCoroutine(follow);
            curState = EnemyState.Angry;

        }
    }

    private void DoAngry()
    {
        // 이동속도를 증가시키고 어그로수치가 가장 높은 player를 쫒는다
        // IEnumerator angry;
        if (curTime > 30)
        {

        }
        else
        {
            // 애니메이션 실행해주기
            curState = EnemyState.Berserker;
        }
    }

    private void DoBerserker()
    {
        // 이동속도를 더욱 더 증가시키고 어그로수치가 가장 높은 player를 쫒는다

        // 애니메이션 실행해주기


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
