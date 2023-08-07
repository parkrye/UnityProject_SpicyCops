using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public class ItemManager : MonoBehaviourPun
{
    public ItemSpot[] ItemSpots = new ItemSpot[1];
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
        if (itemList[index].WeaponType == Define.WeaponType.Util)
        {
            UtilItem uItem = (UtilItem)itemList[index];
            StartCoroutine(uItem.Corutine(player));
        }
    }

    [PunRPC]
    public void RequestGiveRandomItem(int playerId)
    {
        if (!photonView.IsMine)
            return;
        int randNum = (int)Define.ItemIndex.SmokeBomb;
        ItemSpots[0].photonView.RPC("ResultGiveRandomItem", RpcTarget.AllViaServer, playerId, randNum);
    }
    
}
