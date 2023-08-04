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
            // getcomponent 플레이어 검출
            // 자기자신은 리턴
            // 기절 부여(컨트롤러 일정시간 비활성화?) 혹은 이동속도 0
        }
    }
}
