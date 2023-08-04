using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hammer", menuName = "Item/Melee/Hammer")]
public class Hammer : MeleeItem
{
    protected override void MeeleAttack(Vector3 pos, Quaternion rot, float lag, Player player)
    {
        base.MeeleAttack(pos, rot, lag, player);
        foreach(Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<PhotonView>().IsMine)
                return;
            // getcomponent �÷��̾� ����
            // �ڱ��ڽ��� ����
            // ���� �ο�(��Ʈ�ѷ� �����ð� ��Ȱ��ȭ?) Ȥ�� �̵��ӵ� 0
        }
    }
}
