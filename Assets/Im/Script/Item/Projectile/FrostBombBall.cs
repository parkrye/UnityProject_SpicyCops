using Photon.Pun;
using UnityEngine;

public class FrostBombBall : BallBase
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
            // ������ġ ���� ����Ʈ �� ���� ����
            Collider[] colliders = Physics.OverlapSphere(transform.position, overlapAreaRange);
            foreach (Collider collider in colliders)
            {
                PlayerMover mover = collider.GetComponent<PlayerMover>();
                if (mover != null)
                {
                    mover.RequestSpeedDown(mover.photonView.ViewID);
                }
            }
        }
        Destroy(gameObject, 2f);
    }
}
