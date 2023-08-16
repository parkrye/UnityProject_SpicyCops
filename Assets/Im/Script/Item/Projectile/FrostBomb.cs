using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FrostBomb", menuName = "Item/Projectile/FrostBomb")]
public class FrostBomb : ProjectileItem
{
    protected override void Projectile(Vector3 pos, Quaternion rot, float lag, int viewId)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        GameObject ball = PhotonNetwork.Instantiate("FrostBomb", pos, rot);
        BallBase b = ball.GetComponent<BallBase>();
        b.SetPlayer(viewId);
    }
}
