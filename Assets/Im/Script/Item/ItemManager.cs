using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviourPun
{
    public Item[] itemList = new Item[(int)Define.ItemIndex.Count];
    private void Awake()
    {
        for(int i = 0; i < itemList.Length; i++)
        {
            Item item = GameManager.Resource.Load<Item>($"Item/{(Define.ItemIndex)i}");
            if(item != null)
                itemList[i] = item;
        }
    }

    [PunRPC]
    protected void RequestUseItem(Vector3 pos, Quaternion rot, int index, PhotonMessageInfo info)
    {
        float sentTime = (float)info.SentServerTime;
        photonView.RPC("ResultUseItem", RpcTarget.AllViaServer, pos, rot, sentTime, info.Sender, index);
    }
    [PunRPC]
    protected void ResultUseItem(Vector3 pos, Quaternion rot, float sentTime, Player player, int index)
    {
        float lag = (float)(PhotonNetwork.Time - sentTime);
        itemList[index].UseItem(pos, rot, lag, player);
    }
}
