using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoes : UtilItem
{
    protected override void Awake()
    {
        base.Awake();
        itemIcon = Resources.Load<Sprite>("ShoesIcon");
        itemIndex = (int)Define.ItemIndex.Shoes;
        itemName = "Shoes";
        itemList[itemIndex] = this;
    }
    protected override void GetUtilEffect(Player player)
    {
        StartCoroutine(SpeedUp(player));
    }
    IEnumerator SpeedUp(Player player)
    {
        // 플레이어 속도증가
        yield return new WaitForSeconds(5);
        // 원래속도로
    }
}
