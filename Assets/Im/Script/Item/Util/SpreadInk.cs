using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpreadInk", menuName = "Item/Util/SpreadInk")]
public class SpreadInk : UtilItem
{
    protected override void GetUtilEffect(int viewId, Player sender)
    {
        if(PhotonNetwork.IsMasterClient)
            gameManager.DrawEffect(sender.ActorNumber);
    }
}