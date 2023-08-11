using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(fileName = "Hammer", menuName = "Item/Melee/Hammer")]
public class Hammer : MeleeItem
{
    protected override void MeeleAttack(Vector3 pos, Quaternion rot, float lag, int viewId)
    {
        GameObject obj = GameManager.Resource.Instantiate<GameObject>("Slash", pos, rot);
        obj.transform.Rotate(new Vector3(0,40,0), Space.Self);

        Collider[] colliders = Physics.OverlapBox(pos + center, attackArea / 2, rot, mask);
        Debug.Log("¸ÁÄ¡");
        if (colliders == null || colliders.Length < 1)
            return;
        foreach (Collider collider in colliders)
        {
            Debug.Log($"Hammer start : {collider.gameObject.name}");
            PhotonView v = collider.gameObject.GetComponent<PhotonView>();
            if (v == null)
                return;
            if (v.ViewID == viewId)
                return;
            PlayerMover mover = collider.gameObject.GetComponent<PlayerMover>();
            if (mover == null)
                return;
            mover.photonView.RPC("OnStun", RpcTarget.AllViaServer, viewId);
            Debug.Log($"Hammer End : {mover.gameObject.name}");
        }
    }
}
