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

        private float curTime;            // 현재시간은 게임내에서 시간을 적용시킨다 약 : 처음 시간은 180초

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            catchZone = GameObject.Find("CatchZone").transform;
            curTime = 180f;      // time = InGameManager에서 받아오기
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
            // 게임을 시작할 때 잠깐 몇초동안 가만히 있는다
            if (curTime == 175f)
            {
                // 애니메이션 실행해주기
                anim.SetBool("WalkTime", true);
                curState = EnemyState.Follow;
                
            }
        }
        private void DoFollow()
        {
            // Player중에 어그로수치가 가장 높은 player를 쫒는다
            if (curTime == 120f)
            {
                // 애니메이션 실행해주기
                anim.SetBool("WalkTime", false);
                anim.SetBool("RunningTime", true);
                agent.speed = 6f;
                curState = EnemyState.Angry;

            }
        }

        private void DoAngry()
        {
            // 이동속도를 증가시키고 어그로수치가 가장 높은 player를 쫒는다
            // IEnumerator angry;
            if (curTime == 60f)
            {
                // 애니메이션 실행해주기
                agent.speed = 8f;
                curState = EnemyState.Berserker;
            }
        }

        private void DoBerserker()
        {
            // 이동속도를 더욱 더 증가시키고 어그로수치가 가장 높은 player를 쫒는다

            // 애니메이션 실행해주기
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
            // 사정거리를 만들어서 안에 들어오면 anim.SetBool("Area", true);로 애니메이션 실행과 플레이어 사망

        }
    }

}
