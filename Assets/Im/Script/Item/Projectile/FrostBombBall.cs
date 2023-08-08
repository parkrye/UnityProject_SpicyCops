using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostBombBall : BallBase
{
    [SerializeField] GameObject effect;

    [PunRPC]
    protected override void ResultExplosion(Vector3 pos, Quaternion rot, float sentTime)
    {
        Instantiate(effect, pos, Quaternion.identity);
        if (!PhotonNetwork.IsMasterClient)
            return;
        // ������ġ ���� ����Ʈ �� ���� ����
        Collider[] colliders = Physics.OverlapSphere(transform.position, overlapAreaRange);
        foreach (Collider collider in colliders)
        {
            PhotonView view = collider.GetComponent<PhotonView>();
            PlayerMover mover = collider.GetComponent<PlayerMover>();
            if (view != null && mover != null)
            {
                StartCoroutine(SlowDown(mover));
            }
        }
        Destroy(gameObject, 3f);
    }
    IEnumerator SlowDown(PlayerMover mover)
    {
        float speed = mover.moveSpeed;
        float rate = 0;
        while(rate < 1)
        {
            mover.moveSpeed = Mathf.Lerp(0, speed, rate);
            rate += Time.deltaTime / 3;
            yield return new WaitForEndOfFrame();
        }
        mover.moveSpeed = speed;
    }
}
