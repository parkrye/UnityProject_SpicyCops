using Photon.Pun;
using UnityEngine;

public class InGameUI_PlayerEntry : SceneUI
{
    [SerializeField] InGameUI_RadialAggro inGameUI_RadialAggro;
    [SerializeField] PhotonView playerPhotonView;

    public void SetPlayerPhotonView(PhotonView player)
    {
        playerPhotonView = player;
    }
}
