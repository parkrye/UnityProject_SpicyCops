using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkBomb : ProjectileItem
{
    protected override void Awake()
    {
        base.Awake();
        itemIcon = Resources.Load<Sprite>("Sprite/SmokeBombIcon");
        itemIndex = (int)Define.ItemIndex.InkBomb;
        itemName = "InkBomb";
        itemList[itemIndex] = this;
    }
    protected override void Projectile(Vector3 pos, Quaternion rot, float sentTime, Player player)
    {
        GameObject ball = PhotonNetwork.Instantiate("InkBomb", pos, rot);
        BallBase b = ball.GetComponent<BallBase>();
        b.SetPlayer(player);
    }
}
