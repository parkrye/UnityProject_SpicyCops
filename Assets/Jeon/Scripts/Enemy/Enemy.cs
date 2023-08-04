using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEditorInternal.VersionControl.ListControl;

namespace Jeon
{
    public abstract class Enemy : MonoBehaviour
    {
        private Rigidbody rb;
        private NavMeshAgent agent;
        private Vector3 followPos;
        public Animator anim;

        Dictionary<string, int> playerState = new Dictionary<string, int>();

        [SerializeField] Transform players;
        [SerializeField] Transform catchZone;
        [SerializeField] float targetFindTime;

        public enum EnemyState { Idle, Follow, Angry, Berserker, Catch}

        private EnemyState curState;
        private EnemyState lastState;

        [SerializeField] float curTime;            // 현재시간은 게임내에서 시간을 적용시킨다 약 : 처음 시간은 180초
        [SerializeField] float curSpeed;

        public void Awake()
        {
            rb = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            //catchZone = GameObject.Find("CatchZone").transform;
            curTime = 180f;      // time = InGameManager에서 받아오기
            curSpeed = agent.speed;
        }
        private void Start()
        {
            players = GameObject.FindGameObjectWithTag("Player").transform;

            StartCoroutine(FinedPlayer());
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
            if (curTime <= 175f)
            {
                curState = EnemyState.Follow;
                
            }
            StopCoroutine(FinedPlayer());
        }
        private void DoFollow()
        {
            anim.SetBool("WalkTime", true);
            agent.speed = 3f;

            // Player중에 어그로수치가 가장 높은 player를 쫒는다
            if (curTime <= 120f)
            {
                curState = EnemyState.Angry;

            }
        }

        private void DoAngry()
        {
            // 이동속도를 증가시키고 어그로수치가 가장 높은 player를 쫒는다
            agent.speed = 3.5f;
            anim.SetBool("WalkTime", false);
            anim.SetBool("RunningTime", true);

            if (curTime <= 60f)
            {
                curState = EnemyState.Berserker;
            }
        }

        private void DoBerserker()
        {
            agent.speed = 4f;
            if (curTime <= 15f)
            {
                agent.speed = 6f;
            }
            else if (curTime <= 0)
            {
                agent.speed = 0f;
                StopAllCoroutines();
            }
        }

        private void DoCatch()
        {
            curSpeed = agent.speed;
            lastState = curState;

            agent.speed = 0.1f;
        }
        
        private void Remove()
        {
            curState = EnemyState.Idle;
            agent.speed = curSpeed;
            curState = lastState;
        }

        IEnumerator FinedPlayer()
        {
            yield return new WaitForSeconds(5f);
            
            curState = EnemyState.Idle;
            curTime = 175f;
            yield return null;
            while (true)
            {
                agent.destination = players.transform.position;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

}
