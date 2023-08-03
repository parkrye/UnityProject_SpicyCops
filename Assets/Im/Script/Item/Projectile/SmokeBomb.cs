using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBomb : ProjectileItem
{
    protected override void Awake()
    {
        base.Awake();
        itemIcon = Resources.Load<Sprite>("Sprite/SmokeBombIcon");
        itemIndex = (int)Define.ItemIndex.SmokeBomb;
        itemName = "SmokeBomb";
        itemList[itemIndex] = this;
    }
    protected override void Projectile(Vector3 pos, Quaternion rot, float sentTime, Player player)
    {
        GameObject ball = PhotonNetwork.Instantiate("SmokeBomb", pos, rot);
        BallBase b = ball.GetComponent<BallBase>();
        b.SetPlayer(player);
    }
}
