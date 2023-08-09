using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Hammer", menuName = "Item/Melee/Hammer")]
public class Hammer : MeleeItem
{
    protected override void MeeleAttack(Vector3 pos, Quaternion rot, float lag, int viewId)
    {
        Collider[] colliders = Physics.OverlapBox(pos, attackArea / 2.5f, rot);
        Debug.Log("¸ÁÄ¡");
        if (colliders == null || colliders.Length < 1)
            return;
        foreach (Collider collider in colliders)
        {
            PhotonView v = collider.gameObject.GetComponent<PhotonView>();
            if (v == null)
                return;
            if (v.ViewID == viewId)
                return;
            PlayerMover mover = collider.gameObject.GetComponent<PlayerMover>();
            if (mover == null)
                return;
            mover.OnStun();
        }
    }
}
