using Photon.Pun.Demo.Asteroids;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public abstract class Item : MonoBehaviourPun
{
    public Item[] itemList;
    protected string itemName;
    protected Sprite itemIcon;
    protected int itemIndex;
    protected Define.WeaponType weaponType;
    public string ItemName { get { return itemName; } }
    public Sprite ItemIcon { get { return itemIcon; } }
    public int ItemIndex { get { return itemIndex; } }
    private void Awake()
    {
        itemList = new Item[(int)Define.ItemIndex.Count];
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
    public abstract void UseItem(Vector3 pos, Quaternion rot, float lag, Player player);
}