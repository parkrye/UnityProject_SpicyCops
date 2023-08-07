using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpreadInk", menuName = "Item/Util/SpreadInk")]
public class SpreadInk : UtilItem
{
    protected override void GetUtilEffect(Player player)
    {
        // 코루틴
    }
    public override IEnumerator Corutine(Player player)
    {
        // 화면가리기
        yield return new WaitForSeconds(5);
        // 치우기
    }
}