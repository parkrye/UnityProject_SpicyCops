using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SmokeBomb", menuName = "Item/Projectile/SmokeBomb")]
public class SmokeBomb : ProjectileItem
{
    protected override void Projectile(Vector3 pos, Quaternion rot, float sentTime, int viewId)
    {
        GameObject ball = PhotonNetwork.Instantiate("SmokeBomb", pos, rot);
        BallBase b = ball.GetComponent<BallBase>();
        b.SetPlayer(viewId);
    }
}
