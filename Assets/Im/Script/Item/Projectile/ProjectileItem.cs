using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileItem : Item
{
    protected virtual void Awake()
    {
        weaponType = Define.WeaponType.Projectile;
    }

    public override void UseItem(Vector3 pos, Quaternion rot, float lag, Player player)
    {
        Projectile(pos, rot, lag, player);
    }
    protected virtual void Projectile(Vector3 pos, Quaternion rot, float lag, Player player) 
    { 

    }
}
