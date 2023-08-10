using Photon.Realtime;
using UnityEngine;

public class UtilItem : Item
{
    protected virtual void GetUtilEffect(int viewId, Player sender)
    {

    }
    public override void UseItem(Vector3 pos, Quaternion rot, float lag, int viewId, Player sender)
    {
        GetUtilEffect(viewId, sender);
    }
}