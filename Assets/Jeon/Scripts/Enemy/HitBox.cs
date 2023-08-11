using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitBox : MonoBehaviourPun
{
    [SerializeField] private PoliceEnemy enemy;
    [SerializeField] InGameManager inGameManager;
    [SerializeField] List<int> playerViewIdList;

    private void Awake()
    {
        enemy = GetComponentInParent<PoliceEnemy>();
        playerViewIdList = new();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + other.gameObject.layer);
        if (other.gameObject.layer == 3)
        {
            photonView.RPC("RequestAddPlayer", RpcTarget.MasterClient, other.GetComponent<PhotonView>().ViewID);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            photonView.RPC("RequestExitPlayer", RpcTarget.MasterClient, other.GetComponent<PhotonView>().ViewID);
        }

    }
    [PunRPC]
    private void RequestAddPlayer(int playerId)
    {
        playerViewIdList.Add(playerId);
        Debug.Log($"{playerId}");
        photonView.RPC("RequestHoldPlayer", RpcTarget.MasterClient, playerId);
    }

    [PunRPC]
    private void RequestHoldPlayer(int playerId)
    {
        foreach (int view in playerViewIdList)
        {
            if (view == playerId)
            {
                Debug.Log($"{playerId}");
                enemy.Anim.SetBool("InArea", true);
                // Debug.Log(enemy.Anim.GetBool("InArea"));
                enemy.playerTransform[view].GetComponent<PlayerDied>().DoDeath();
                inGameManager.PlayerDead(view);
                enemy.playerTransform[view].GetComponent<PlayerInput>().enabled = false;
                StartCoroutine(EnemyAttackStop());
            }
        }
    }
    [PunRPC]
    private void RequestExitPlayer(int playerId)
    {
        playerViewIdList.Remove(playerId);
    }
    IEnumerator EnemyAttackStop()
    {
        yield return new WaitForSeconds(1f);
        enemy.Anim.SetBool("InArea", false);
    }
}