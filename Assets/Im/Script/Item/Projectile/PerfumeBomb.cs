using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PerfumeBomb", menuName = "Item/Projectile/PerfumeBomb")]
public class PerfumeBomb : ProjectileItem
{
    protected override void Projectile(Vector3 pos, Quaternion rot, float sentTime, int viewId)
    {
        GameObject ball = PhotonNetwork.Instantiate("PerfumeBomb", pos, rot);
        BallBase b = ball.GetComponent<BallBase>();
        b.SetPlayer(viewId);
    }
}
