using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoes", menuName = "Item/Util/Shoes")]
public class Shoes : UtilItem
{
    protected override void GetUtilEffect(Player player)
    {
        // �ڷ�ƾ ���� ��û
    }
    IEnumerator SpeedUp(Player player)
    {
        // �÷��̾� �ӵ�����
        yield return new WaitForSeconds(5);
        // �����ӵ���
    }
}
