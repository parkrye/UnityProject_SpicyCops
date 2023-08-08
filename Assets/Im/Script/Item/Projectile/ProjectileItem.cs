using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileItem : Item
{
    public override void UseItem(Vector3 pos, Quaternion rot, float lag, int viewId)
    {
        Projectile(pos, rot, lag, viewId);
    }
    protected virtual void Projectile(Vector3 pos, Quaternion rot, float lag, int viewId) 
    { 

    }
}
