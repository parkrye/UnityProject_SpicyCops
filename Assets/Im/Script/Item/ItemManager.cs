using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ItemManager : MonoBehaviourPun
{
    public ItemSpot[] itemSpots = new ItemSpot[16];
    public Item[] itemList = new Item[(int)Define.ItemIndex.Count];
    public InGameManager gameManager;

    private void Awake()
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            Item item = GameManager.Resource.Load<Item>($"Item/{(Define.ItemIndex)i}");
            if (item != null)
            {
                itemList[i] = item;
                item.gameManager = gameManager;
            }
        }
        for (int i = 0; i < itemSpots.Length; i++)
        {
            // itemSpots[i].manager = gameManager;
            itemSpots[i].itemSpotIndex = i;
        }
    }
    public void Init()
    {
        foreach (var item in itemSpots) 
        {
            item.Init();
        }
    }

    [PunRPC]
    public void RequestUseItem(Vector3 pos, Quaternion rot, int index, int viewId, PhotonMessageInfo info)
    {
        float sentTime = (float)info.SentServerTime;
        photonView.RPC("ResultUseItem", RpcTarget.AllViaServer, pos, rot, sentTime, viewId, index, info.Sender);
    }
    [PunRPC]
    public void ResultUseItem(Vector3 pos, Quaternion rot, float sentTime, int viewId, int index, Player sender)
    {
        float lag = (float)(PhotonNetwork.Time - sentTime);
        itemList[index].UseItem(pos, rot, lag, viewId, sender);
        StartCoroutine(itemList[index].PlaySFX(pos));
    }

    
    
}
