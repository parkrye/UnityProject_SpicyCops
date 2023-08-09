using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Hammer", menuName = "Item/Melee/Hammer")]
public class Hammer : MeleeItem
{
    protected override void MeeleAttack(Vector3 pos, Quaternion rot, float lag, int viewId)
    {
        Collider[] colliders = Physics.OverlapBox(pos, attackArea / 2.5f, rot);
        Debug.Log("망치");
        if (colliders == null || colliders.Length < 1)
            return;
        foreach (Collider collider in colliders)
        {
            PhotonView v = collider.gameObject.GetComponent<PhotonView>();
            if (v == null)
                return;
            if (v.ViewID == viewId)
                return;
            PlayerMover mover = collider.gameObject.GetComponent<PlayerMover>();
            if (mover == null)
                return;
            Debug.Log("기절");
            // getcomponent 플레이어 검출
            // 자기자신은 리턴
            // 기절 부여(컨트롤러 일정시간 비활성화?) 혹은 이동속도 0
        }
    }
}
