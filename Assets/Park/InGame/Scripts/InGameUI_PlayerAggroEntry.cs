using Photon.Pun;
using UnityEngine;

public class InGameUI_PlayerAggroEntry : SceneUI
{
    [SerializeField] InGameUI_RadialAggro inGameUI_RadialAggro;
    [SerializeField] PhotonView targetPlayerPhotonView;

    public void SetTargetPlayerPhotonView(PhotonView targetPlayer)
    {
        targetPlayerPhotonView = targetPlayer;
        inGameUI_RadialAggro.Initialize();
    }

    public void ModifiyAggro(float value)
    {
        inGameUI_RadialAggro.ModifyAggro(value);
    }
}
