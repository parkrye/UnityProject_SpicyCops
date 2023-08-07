using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoes", menuName = "Item/Util/Shoes")]
public class Shoes : UtilItem
{
    protected override void GetUtilEffect(Player player)
    {
        // 코루틴 실행 요청
    }
    public override IEnumerator Corutine(Player player)
    {
        // 속도 증가
        yield return new WaitForSeconds(5);
        // 원래대로
    }
}
