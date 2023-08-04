using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmokeBombBall : BallBase
{
    [SerializeField] GameObject effect;

    [PunRPC]
    protected override void ResultExplosion(Vector3 pos, Quaternion rot, float sentTime)
    {
        Instantiate(effect);
        if (!PhotonNetwork.IsMasterClient)
            return;
        // 현재위치 기준 이펙트 및 사운드 적용
        Collider[] colliders = Physics.OverlapSphere(transform.position, overlapAreaRange, mask);
        foreach (Collider collider in colliders)
        {
            // PlayerController player =  collider.GetComponent<PlayerController>
            /* if(player != null)
             *     player.speed -= 100; 
             */
            // 아무튼 효과 부여
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, overlapAreaRange);
    }
}
