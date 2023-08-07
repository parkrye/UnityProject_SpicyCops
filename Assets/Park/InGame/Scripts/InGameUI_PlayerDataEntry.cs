using Photon.Pun;
using UnityEngine;

public class InGameUI_PlayerDataEntry : SceneUI
{
    [SerializeField] InGameUI_RadialAggro inGameUI_RadialAggro;
    [SerializeField] PhotonView targetPlayerPhotonView;
    [SerializeField] GameObject deadImage;

    public void SetTargetPlayerPhotonView(PhotonView targetPlayer)
    {
        targetPlayerPhotonView = targetPlayer;
        inGameUI_RadialAggro.Initialize();
    }

    public void ModifiyAggro(float value)
    {
        inGameUI_RadialAggro.ModifyAggro(value);
    }

    public void CheckAlive(bool alive)
    {
        deadImage.SetActive(!alive);
    }
}
