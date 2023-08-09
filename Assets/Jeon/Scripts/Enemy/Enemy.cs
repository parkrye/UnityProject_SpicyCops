using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Jeon
{
    public class Enemy : MonoBehaviourPun
    {
        [SerializeField] Transform player;  

        private NavMeshAgent agent;
        private Animator anim;

        [SerializeField] PhotonView _photonView;

        public Animator Anim { get { return anim; } }
        [SerializeField] InGameManager inGameManager;

        Dictionary<string, int> playerState = new Dictionary<string, int>();
        public Dictionary<int, Transform> playerTransform;


        [SerializeField] Transform catchZone;
        [SerializeField] float targetFindTime;

        int maxAggroViewID;
        float moveSpeed;

        public enum EnemyState { Idle, Follow, Angry, SemiBerserker, Berserker, End, Catch}

        private EnemyState curState;
        private EnemyState lastState;

        [SerializeField] float curTime;            // 현재시간은 게임내에서 시간을 적용시킨다 약 : 처음 시간은 180초
        [SerializeField] float curSpeed;

        [SerializeField] Material material;

        private void SetServerTime(float time)
        {
            _photonView.RPC("RequestEnemyMoveSetting", RpcTarget.MasterClient, time);

        }

        [PunRPC]
        protected void RequestEnemyMoveSetting(float time)    // 에이전트 스피드가 되는지 RPC Time.
        {
            curTime = time;
            if (curTime >= 5f && curState == EnemyState.Idle)
            {
                DoFollow();
            }
            else if (curTime >= inGameManager.TotalTime * 0.3f && curState == EnemyState.Follow)
            {
                DoAngry();
            }
            else if (curTime >= inGameManager.TotalTime * 0.6f && curState == EnemyState.Angry)
            {
                DoSemiBerserker();
            }
            else if (curTime >= inGameManager.TotalTime - 15f && curState == EnemyState.SemiBerserker)
            {
                DoBerserker();
            }
            else if (curTime >= inGameManager.TotalTime && curState == EnemyState.Berserker)
            {
                DoEndGame();
            }
        }
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            //catchZone = GameObject.Find("CatchZone").transform;
            curSpeed = agent.speed;
            material.color = Color.white;
            playerTransform = new Dictionary<int, Transform>();
        }

        private void SetPlayerAggro(Dictionary<int, float> playerAggro) //아이디와 어그로수치
        {
            int ViewID = 0;
            float highAggro = -1;
            foreach (KeyValuePair<int, float> keyValuePair in playerAggro)
            {
                if (highAggro < keyValuePair.Value)
                {
                    highAggro = keyValuePair.Value;
                    ViewID = keyValuePair.Key;
                }
            }
            maxAggroViewID = ViewID;
        }

        public void Seting()
        {
            inGameManager.AddTimeEventListener(SetServerTime);

            inGameManager.AddPlayerAggroEventListener(SetPlayerAggro);

            foreach (int playerViewID in inGameManager.PlayerAggroDictionary.Keys)
            {
                playerTransform.Add(playerViewID, PhotonView.Find(playerViewID).transform);
                inGameManager.ModifyPlayerAggro(playerViewID, 0f);
            }
            StartCoroutine(FindPlayer());
        }

        private void DoFollow()
        {
            anim.SetBool("WalkTime", true);
            agent.speed = 3f;

            curState = EnemyState.Follow;
        }

        private void DoAngry()
        {
            agent.speed = 5f;
            anim.SetBool("WalkTime", false);
            anim.SetBool("RunningTime", true);

            curState = EnemyState.Angry;
            material.color = new Color(1, 0.65f, 0.65f);
        }

        private void DoSemiBerserker()
        {
            agent.speed = 7.5f;

            curState = EnemyState.SemiBerserker;
            material.color = new Color(1, 0.45f, 0.45f);
        }

        private void DoBerserker()
        {
            agent.speed = 9.5f;
            material.color = new Color(0.24f, 0f, 0f);

            curState = EnemyState.Berserker;
        }

        private void DoEndGame()
        {
            agent.speed = 0f;
            curState = EnemyState.End;

            material.color = Color.white;

            StopAllCoroutines();
        }

        /*private void DoCatch()
        {
            curSpeed = agent.speed;

            agent.speed = 0.1f;
        }
        
        private void Remove()
        {
            curState = EnemyState.Idle;
            agent.speed = curSpeed;
            curState = lastState;
        }*/

        IEnumerator FindPlayer()
        {
            yield return new WaitForSeconds(5f);
            
            curState = EnemyState.Idle;
            yield return null;
            while (true)
            {
                agent.destination = playerTransform[maxAggroViewID].position;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

}
