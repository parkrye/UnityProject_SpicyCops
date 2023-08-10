using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeItem : Item
{
    [SerializeField] protected LayerMask mask;
    protected Vector3 attackArea = new Vector3 (2, 2, 2);
    protected Vector3 center = new Vector3(0, 1, 1);
    
    protected virtual void MeeleAttack(Vector3 pos, Quaternion rot, float lag, int viewId)
    {
        
    }
    public override void UseItem(Vector3 pos, Quaternion rot, float lag, int viewId, Player sender)
    {
        MeeleAttack(pos, rot, lag, viewId);
    }
}
