using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeItem : Item
{
    [SerializeField] protected LayerMask mask;
    [SerializeField] protected Transform attackPos;

    protected Vector3 attackArea = new Vector3 (2, 3, 2);
    protected Collider[] colliders;
    
    protected virtual void Awake()
    {
        weaponType = Define.WeaponType.Melee;
    }
    protected virtual void MeeleAttack(Vector3 pos, Quaternion rot, float lag, Player player)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        colliders = Physics.OverlapBox(pos + new Vector3(0, 1.5f, -0.5f), attackArea / 2, rot, mask);
        /*
        foreach (Collider collider in colliders)
        {
            Vector3 targetDir = (collider.transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, targetDir) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
                continue;
            IHittable hittable = collider.GetComponent<IHittable>();
            // hittable?.TakeHit(damage);
        }
        */
    }
    public override void UseItem(Vector3 pos, Quaternion rot, float lag, Player player)
    {
        MeeleAttack(pos, rot, lag, player);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position + new Vector3(0, -0.5f, -0.5f), attackArea);
    }

}
