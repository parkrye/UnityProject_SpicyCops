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
        Instantiate(explosionEff, pos, rot);
        Instantiate(effect, pos, Quaternion.identity);
        AudioSource source = GameManager.Resource.Instantiate<AudioSource>("AudioSource", pos, Quaternion.identity);
        source.PlayOneShot(clip);
        GameManager.Resource.Destroy(source, 2f);
        if (PhotonNetwork.IsMasterClient)
        {
            // ������ġ ���� ����Ʈ �� ���� ����
            Collider[] colliders = Physics.OverlapSphere(transform.position, overlapAreaRange);
            foreach (Collider collider in colliders)
            {
                PhotonView view = collider.GetComponent<PhotonView>();
                PlayerMover mover = collider.GetComponent<PlayerMover>();
                if (view != null && mover != null)
                {
                    gameManager.ModifyPlayerAggro(view.ViewID, -10);
                }
            }
        }
        Destroy(gameObject, 2f);
    }
}