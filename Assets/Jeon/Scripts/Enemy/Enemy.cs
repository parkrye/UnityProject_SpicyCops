using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static Unity.VisualScripting.StickyNote;

namespace Jeon
{
    public class Enemy : MonoBehaviourPun
    {
        [SerializeField] Transform player;

        private NavMeshAgent agent;
        private Animator anim;

        [SerializeField] Color color1 = new Color(1f, 1f, 1f);
        [SerializeField] Color color2 = new Color(1f, 0.65f, 0.65f);
        [SerializeField] Color color3 = new Color(1, 0.35f, 0.35f);
        [SerializeField] Color color4 = new Color(0.24f, 0, 0);

        public Animator Anim { get { return anim; } }
        [SerializeField] InGameManager inGameManager;

        Dictionary<string, int> playerState = new Dictionary<string, int>();
        public Dictionary<int, Transform> playerTransform;


        [SerializeField] Transform catchZone;
        [SerializeField] float targetFindTime;

        int maxAggroViewID;

        public enum EnemyState { Idle, Follow, Angry, SemiBerserker, Berserker, End, Catch}

        private EnemyState curState;

        [SerializeField] float curTime = 0;            // ����ð��� ���ӳ����� �ð��� �����Ų�� �� : ó�� �ð��� 180��

        [SerializeField] Material material;

        private void SetTime()
        {
            if (PhotonNetwork.IsMasterClient)
                RequestEnemyMoveSetting();
        }

        private void RequestEnemyMoveSetting()    // ������Ʈ ���ǵ尡 �Ǵ��� RPC Time.
        {
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

            if (curTime >= inGameManager.TotalTime)
            {
                StopAllCoroutines();
                anim.enabled = false;
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }
        }


        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            //catchZone = GameObject.Find("CatchZone").transform;
            material.color = Color.white;
            playerTransform = new Dictionary<int, Transform>();
        }

        private void SetPlayerAggro(Dictionary<int, float> playerAggro) //���̵�� ��׷μ�ġ
        {
            int ViewID = 0;
            float highAggro = -1;
            foreach (KeyValuePair<int, float> keyValuePair in playerAggro)
            {
                if (!inGameManager.PlayerAliveDictionary[keyValuePair.Key])
                    continue;
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
            StartCoroutine(TimeRoutine());

            inGameManager.AddPlayerAggroEventListener(SetPlayerAggro);

            foreach (int playerViewID in inGameManager.PlayerAggroDictionary.Keys)
            {
                playerTransform.Add(playerViewID, PhotonView.Find(playerViewID).transform);
                inGameManager.ModifyPlayerAggro(playerViewID, 0f);
            }
            StartCoroutine(FindPlayer());
        }

        #region MovePattern
        [PunRPC]
        protected void ColorChange(int colorNum)
        {
            Debug.Log($"{colorNum}");
            if (colorNum == 1)
            {
                material.color = color1;
            }
            else if(colorNum == 2)
            {
                material.color = color2;
            }
            else if (colorNum == 3)
            {
                material.color = color3;
            }
            else
            {
                material.color = color4;
            }
        }
        private void DoFollow()
        {
            anim.SetBool("WalkTime", true);
            agent.speed = 3f;

            curState = EnemyState.Follow;
            Debug.Log($"RequestColor");
            photonView.RPC("ColorChange", RpcTarget.AllViaServer, 1);
            Debug.Log($"ResultColor");
        }

        private void DoAngry()
        {
            agent.speed = 5.5f;
            anim.SetBool("WalkTime", false);
            anim.SetBool("RunningTime", true);

            curState = EnemyState.Angry;
            photonView.RPC("ColorChange", RpcTarget.AllBufferedViaServer, 2);
        }

        private void DoSemiBerserker()
        {
            agent.speed = 8f;

            curState = EnemyState.SemiBerserker;
            photonView.RPC("ColorChange", RpcTarget.AllBufferedViaServer, 3);
        }

        private void DoBerserker()
        {
            agent.speed = 10f;
            photonView.RPC("ColorChange", RpcTarget.AllBufferedViaServer, 4);

            curState = EnemyState.Berserker;
        }

        private void DoEndGame()
        {
            agent.speed = 0f;
            curState = EnemyState.End;

            material.color = Color.white;

            StopAllCoroutines();
        }
        #endregion
        IEnumerator FindPlayer()
        {
            yield return new WaitForSeconds(5f);
            
            curState = EnemyState.Idle;
            yield return null;
            while (true)
            {
                agent.destination = playerTransform[maxAggroViewID].position;
                yield return null;
            }
        }
        IEnumerator TimeRoutine()
        {
            while (true)
            {
                curTime += Time.deltaTime;
                SetTime();
                yield return null;
            }
        }
        #region HitBox
        public void AddPlayer(int viewId)
        {
            Debug.Log($"RequestAddPlayer{viewId}");
            photonView.RPC("RequestAddPlayer", RpcTarget.MasterClient, viewId);

        }
        public void RemovePlayer(int viewId)
        {
            Debug.Log($"RequestRemovePlayer{viewId}");
            photonView.RPC("RequestExitPlayer", RpcTarget.MasterClient, viewId);
        }
        [PunRPC]
        protected void RequestAddPlayer(int playerId)
        {
            Debug.Log($"RequestAddPlayer{playerId}");
            photonView.RPC("RequestHoldPlayer", RpcTarget.MasterClient, playerId);
        }

        [PunRPC]
        protected void RequestHoldPlayer(int playerId)
        {
            if (!inGameManager.PlayerAliveDictionary[playerId])
                return;
            Debug.Log($"RequestHoldPlayer{playerId}");
            Anim.SetBool("InArea", true);
            Debug.Log(Anim.GetBool("InArea"));
            playerTransform[playerId].GetComponent<PlayerDied>().DoDeath();
            inGameManager.PlayerDead(playerId);
            playerTransform[playerId].GetComponent<PlayerInput>().enabled = false;
            StartCoroutine(EnemyAttackStop());
            SetPlayerAggro(inGameManager.PlayerAggroDictionary);
        }
        IEnumerator EnemyAttackStop()
        {
            yield return new WaitForSeconds(1f);
            Anim.SetBool("InArea", false);
        }
        #endregion
    }

}
