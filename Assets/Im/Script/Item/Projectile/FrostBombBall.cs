using Photon.Pun;
using UnityEngine;

public class FrostBombBall : BallBase
{
    [SerializeField] GameObject effect;

    [PunRPC]
    protected override void ResultExplosion(Vector3 pos, Quaternion rot, float sentTime)
    {
        Instantiate(effect, pos, Quaternion.identity);
        if (photonView.IsMine)
        {
            // ������ġ ���� ����Ʈ �� ���� ����
            Collider[] colliders = Physics.OverlapSphere(transform.position, overlapAreaRange);
            foreach (Collider collider in colliders)
            {
                PhotonView view = collider.GetComponent<PhotonView>();
                PlayerMover mover = collider.GetComponent<PlayerMover>();
                if (view != null && mover != null)
                {
                    mover.photonView.RPC("OnSpeedDown", RpcTarget.AllBufferedViaServer, viewId);
                }
            }
        }
        Destroy(gameObject, 3f);
    }
}
