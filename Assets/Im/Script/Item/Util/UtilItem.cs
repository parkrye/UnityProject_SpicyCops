using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilItem : Item
{
    protected virtual void GetUtilEffect(int viewId)
    {

    }
    public override void UseItem(Vector3 pos, Quaternion rot, float lag, int viewId, Player sender)
    {
        GetUtilEffect(viewId);
    }
}