using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MeleeItem
{
    protected override void Awake()
    {
        base.Awake();
        itemIcon = Resources.Load<Sprite>("Sprite/HammerIcon");
        itemIndex = (int)Define.ItemIndex.Hammer;
        itemName = "Hammer";
        itemList[itemIndex] = this;
    }
    protected override void MeeleAttack(Vector3 pos, Quaternion rot, float lag, Player player)
    {
        base.MeeleAttack(pos, rot, lag, player);
        foreach(Collider collider in colliders)
        {
            // getcomponent 플레이어 검출
            // 기절 부여(컨트롤러 일정시간 비활성화?)
        }
    }
}
