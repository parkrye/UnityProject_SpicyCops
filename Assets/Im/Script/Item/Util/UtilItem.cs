using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilItem : Item
{
    protected virtual void GetUtilEffect(Player player)
    {

    }
    public override void UseItem(Vector3 pos, Quaternion rot, float lag, Player player)
    {
        GetUtilEffect(player);
    }

}