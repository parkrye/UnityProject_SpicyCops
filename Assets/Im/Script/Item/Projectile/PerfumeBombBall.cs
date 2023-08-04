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
        Instantiate(effect);
        if (!PhotonNetwork.IsMasterClient)
            return;
        // ������ġ ���� ����Ʈ �� ���� ����
        Collider[] colliders = Physics.OverlapSphere(transform.position, overlapAreaRange);
        foreach (Collider collider in colliders)
        {
            PhotonView view = collider.GetComponent<PhotonView>();
            if (view != null)
            {
                // view.ViewID
                // ��׷� ��ġ ����
            }
        }
    }
}
