using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoes", menuName = "Item/Util/Shoes")]
public class Shoes : UtilItem
{
    protected override void GetUtilEffect(int viewId)
    {
        PhotonView view = PhotonView.Find(viewId);
        PlayerMover mover = view.gameObject.GetComponent<PlayerMover>();
        mover.OnSpeedUp();
    }
}
