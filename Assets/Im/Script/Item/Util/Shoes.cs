using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoes", menuName = "Item/Util/Shoes")]
public class Shoes : UtilItem
{
    protected override void GetUtilEffect(int viewId, Player sender)
    {
        PhotonView view = PhotonView.Find(viewId);
        PlayerMover mover = view.gameObject.GetComponent<PlayerMover>();
        mover.photonView.RPC("OnSpeedUp", RpcTarget.AllBufferedViaServer, viewId);
    }
}
