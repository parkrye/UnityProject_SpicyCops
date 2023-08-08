using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeItem : Item
{
    protected Vector3 attackArea = new Vector3 (2, 3, 2);
    protected Collider[] colliders;
    
    protected virtual void MeeleAttack(Vector3 pos, Quaternion rot, float lag, int viewId)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        colliders = Physics.OverlapBox(pos, attackArea / 2.5f, rot);
    }
    public override void UseItem(Vector3 pos, Quaternion rot, float lag, int viewId, Player sender)
    {
        MeeleAttack(pos, rot, lag, viewId);
    }
    public virtual IEnumerator Cor(int viewId)
    {
        yield return null;
    }
}
