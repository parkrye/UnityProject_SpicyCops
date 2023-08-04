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
            // getcomponent �÷��̾� ����
            // ���� �ο�(��Ʈ�ѷ� �����ð� ��Ȱ��ȭ?)
        }
    }
}
