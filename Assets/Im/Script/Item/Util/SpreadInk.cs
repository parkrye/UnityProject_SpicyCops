using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpreadInk", menuName = "Item/Util/SpreadInk")]
public class SpreadInk : UtilItem
{
    protected override void GetUtilEffect(int viewId)
    {
        // �ڷ�ƾ
    }
    public override IEnumerator Corutine(int viewId, Player player)
    {
        
        // ȭ�鰡����
        yield return new WaitForSeconds(5);
        // ġ���
    }
}