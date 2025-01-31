using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfumeBombBall : BallBase
{
    [SerializeField] GameObject effect;

    [PunRPC]
    protected override void ResultExplosion(Vector3 pos, Quaternion rot, float sentTime)
    {
        Instantiate(effect, pos, Quaternion.identity);
        AudioSource source = GameManager.Resource.Instantiate<AudioSource>("AudioSource", pos, Quaternion.identity);
        source.PlayOneShot(clip);
        GameManager.Resource.Destroy(source, 2f);
        if (PhotonNetwork.IsMasterClient)
        {
            // 현재위치 기준 이펙트 및 사운드 적용
            Collider[] colliders = Physics.OverlapSphere(transform.position, overlapAreaRange);
            foreach (Collider collider in colliders)
            {
                PhotonView view = collider.GetComponent<PhotonView>();
                PlayerMover mover = collider.GetComponent<PlayerMover>();
                if (view != null && mover != null)
                {
                    gameManager.ModifyPlayerAggro(view.ViewID, +10);
                }
            }
        }
        Destroy(gameObject, 2f);
    }
}
