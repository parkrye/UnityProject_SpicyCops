using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitBox : MonoBehaviourPun
{
    [SerializeField] private PoliceEnemy enemy;
    [SerializeField] PhotonView myView;
    [SerializeField] InGameManager inGameManager;
    List<int> playerViewIdList;

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
            myView.RPC("RequestAddPlayer", RpcTarget.MasterClient, other.GetComponent<PhotonView>().ViewID);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            myView.RPC("RequestExitPlayer", RpcTarget.MasterClient, other.GetComponent<PhotonView>().ViewID);
        }

    }
    [PunRPC]
    private void RequestAddPlayer(int playerId)
    {
        playerViewIdList.Add(playerId);
        myView.RPC("RequestHoldPlayer", RpcTarget.MasterClient, playerId);
    }

    [PunRPC]
    private void RequestHoldPlayer(int playerId)
    {
        foreach (int view in playerViewIdList)
        {
            if (view == playerId)
            {
                enemy.Anim.SetTrigger("InArea");
                enemy.playerTransform[view].GetComponent<PlayerDied>().DoDeath();
                inGameManager.PlayerDead(view);
                enemy.playerTransform[view].GetComponent<PlayerInput>().enabled = false;
            }
        }
    }

    [PunRPC]
    private void RequestExitPlayer(int playerId)
    {
        playerViewIdList.Remove(playerId);
    }

}