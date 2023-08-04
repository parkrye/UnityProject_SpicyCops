using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Jeon
{
    public abstract class Enemy : MonoBehaviour
    {
        private Rigidbody rb;
        private NavMeshAgent agent;
        private Vector3 followPos;
        private Animator anim;

        Dictionary<string, int> playerState = new Dictionary<string, int>();

        [SerializeField] Transform players;
        [SerializeField] Transform catchZone;
        [SerializeField] float moveSpeed;
        [SerializeField] float maxSpeed;
        [SerializeField] float targetFindTime;

        public enum EnemyState { Idle, Follow, Angry, Berserker, Catch}

        private EnemyState curState;

        private float curTime;            // ����ð��� ���ӳ����� �ð��� �����Ų�� �� : ó�� �ð��� 180��

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            catchZone = GameObject.Find("CatchZone").transform;
            curTime = 180f;      // time = InGameManager���� �޾ƿ���
            curState = EnemyState.Idle;
        }

        private void FixedUpdate()
        {
            players = GameObject.FindGameObjectWithTag("Player").transform;
            

            if (curTime == 175f)
            {
                agent.destination = players.transform.position;

            }
            
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
                case EnemyState.Catch:
                    DoCatch();
                    break;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                anim.SetTrigger("InArea");
            }
        }
        private void DoIdle()
        {
            // ������ ������ �� ��� ���ʵ��� ������ �ִ´�
            if (curTime == 175f)
            {
                // �ִϸ��̼� �������ֱ�
                anim.SetBool("WalkTime", true);
                curState = EnemyState.Follow;
                
            }
        }
        private void DoFollow()
        {
            // Player�߿� ��׷μ�ġ�� ���� ���� player�� �i�´�
            if (curTime == 120f)
            {
                // �ִϸ��̼� �������ֱ�
                anim.SetBool("WalkTime", false);
                anim.SetBool("RunningTime", true);
                agent.speed = 6f;
                curState = EnemyState.Angry;

            }
        }

        private void DoAngry()
        {
            // �̵��ӵ��� ������Ű�� ��׷μ�ġ�� ���� ���� player�� �i�´�
            // IEnumerator angry;
            if (curTime == 60f)
            {
                // �ִϸ��̼� �������ֱ�
                agent.speed = 8f;
                curState = EnemyState.Berserker;
            }
        }

        private void DoBerserker()
        {
            // �̵��ӵ��� ���� �� ������Ű�� ��׷μ�ġ�� ���� ���� player�� �i�´�

            // �ִϸ��̼� �������ֱ�
            if (curTime == 15f)
            {
                
                agent.speed = 12f;
            }
            if (curTime == 0)
            {
                agent.speed = 0f;
            }
        }

        private void DoCatch()
        {
            // �����Ÿ��� ���� �ȿ� ������ anim.SetBool("Area", true);�� �ִϸ��̼� ����� �÷��̾� ���

        }
    }

}
