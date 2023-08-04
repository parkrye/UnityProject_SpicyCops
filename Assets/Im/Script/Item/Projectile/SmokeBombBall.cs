using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmokeBombBall : BallBase
{
    [SerializeField] GameObject effect;
    [SerializeField] GameObject explosionEff;

    [PunRPC]
    protected override void ResultExplosion(Vector3 pos, Quaternion rot, float sentTime)
    {
        Instantiate(explosionEff);
        Instantiate(effect);
        if (!PhotonNetwork.IsMasterClient)
            return;
        // 현재위치 기준 이펙트 및 사운드 적용
        Collider[] colliders = Physics.OverlapSphere(transform.position, overlapAreaRange);
        foreach (Collider collider in colliders)
        {
            PhotonView view = collider.GetComponent<PhotonView>();
            if(view != null)
            {
                // view.ViewID
                // 어그로 수치 감소
            }
        }
    }
}
