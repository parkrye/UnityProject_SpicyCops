using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(fileName = "Hammer", menuName = "Item/Melee/Hammer")]
public class Hammer : MeleeItem
{
    protected override void MeeleAttack(Vector3 pos, Quaternion rot, float lag, int viewId)
    {
        GameObject obj = GameManager.Resource.Instantiate<GameObject>("Slash", pos, rot);
        obj.transform.Rotate(new Vector3(0,40,0), Space.Self);

        if (!PhotonNetwork.IsMasterClient)
            return;
        Collider[] colliders = Physics.OverlapBox(pos + center, attackArea / 2, rot, mask);

        if (colliders == null || colliders.Length < 1)
            return;
        foreach (Collider collider in colliders)
        {
            PhotonView v = collider.gameObject.GetComponent<PhotonView>();
            if (v.ViewID == viewId)
                continue;
            PlayerMover mover = collider.gameObject.GetComponent<PlayerMover>();
            if (mover == null)
                continue;
            mover.RequestStun(v.ViewID);
        }
    }
}
