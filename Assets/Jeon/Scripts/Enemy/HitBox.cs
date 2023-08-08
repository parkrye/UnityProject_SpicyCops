using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private PoliceEnemy enemy;
    List<int> playerViewIdList;

    private void Awake()
    {
        enemy = GetComponentInParent<PoliceEnemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            enemy.photonView.RPC("RequestAddPlayer", RpcTarget.MasterClient, other.GetComponent<PhotonView>().ViewID);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            enemy.photonView.RPC("RequestExitPlayer", RpcTarget.MasterClient, other.GetComponent<PhotonView>().ViewID);
        }

    }
    [PunRPC]
    private void RequestAddPlayer(int playerId)
    {
        playerViewIdList.Add(playerId);
        enemy.photonView.RPC("RequestHoldPlayer", RpcTarget.MasterClient, playerId);
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
            }
        }
    }

    [PunRPC]
    private void RequestExitPlayer(int playerId)
    {
        playerViewIdList.Remove(playerId);
    }

}