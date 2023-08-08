using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public class ItemManager : MonoBehaviourPun
{
    public ItemSpot[] itemSpots = new ItemSpot[14];
    public Item[] itemList = new Item[(int)Define.ItemIndex.Count];
    InGameManager gameManager;
    private void Awake()
    {
        gameManager = GameObject.Find("InGameManager").GetComponent<InGameManager>();

        for (int i = 0; i < itemList.Length; i++)
        {
            Item item = GameManager.Resource.Load<Item>($"Item/{(Define.ItemIndex)i}");
            if(item != null)
            {
                itemList[i] = item;
                item.gameManager = gameManager;
            }
        }
        for(int i = 0; i < itemSpots.Length; i++)
        {
            itemSpots[i].itemManager = this;
            itemSpots[i].itemSpotIndex = i;
        }
    }

    [PunRPC]
    protected void RequestUseItem(Vector3 pos, Quaternion rot, int index, int viewId, PhotonMessageInfo info)
    {
        float sentTime = (float)info.SentServerTime;
        photonView.RPC("ResultUseItem", RpcTarget.AllViaServer, pos, rot, sentTime, viewId, index, info.Sender);
    }
    [PunRPC]
    protected void ResultUseItem(Vector3 pos, Quaternion rot, float sentTime, int viewId, int index, Player sender)
    {
        float lag = (float)(PhotonNetwork.Time - sentTime);
        itemList[index].UseItem(pos, rot, lag, viewId, sender);
        if (itemList[index].WeaponType == Define.WeaponType.Util)
        {
            UtilItem uItem = (UtilItem)itemList[index];
            StartCoroutine(uItem.Corutine(viewId, sender));
            return;
        }
        if(itemList[index].WeaponType == Define.WeaponType.Melee)
        {
            MeleeItem mItem = (MeleeItem)itemList[index];
            StartCoroutine(mItem.Cor(viewId));
        }
    }

    [PunRPC]
    public void RequestGiveRandomItem(int playerId, int itemSpotIndex)
    {
        if (!photonView.IsMine)
            return;
        int randNum = UnityEngine.Random.Range(0, itemList.Length);
        itemSpots[itemSpotIndex].photonView.RPC("ResultGiveRandomItem", RpcTarget.AllViaServer, playerId, randNum);
    }
    
}
