using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] public InGameManager inGameManager;

    [SerializeField] PhotonView playerPhotonView;
    [SerializeField] List<PhotonView> otherPlayerPhotonViews;

    public PhotonView PlayerPhotonView {  get { return playerPhotonView; } }
    public List<PhotonView> OtherPlayerPhotonViews { get { return otherPlayerPhotonViews;} }

    [SerializeField] InGameUI_PlayerAggroBar inGameUI_PlayerAggroBar;
    [SerializeField] InGameUI_OtherPlayerZone inGameUI_otherPlayerZone;
    [SerializeField] InGameUI_TimeSlider inGameUI_TimeSlider;

    public void Initialize()
    {
        inGameUI_PlayerAggroBar.Initialize();
        inGameUI_otherPlayerZone.Initialize();
        inGameUI_TimeSlider.Initialize();
        inGameManager.AddPlayerAggroEventListenr(ModifyAggroUIValue);
    }

    public void SetPlayerPhotonView(PhotonView player)
    {
        playerPhotonView = player;
        for(int i = 0; i < otherPlayerPhotonViews.Count; i++)
        {
            if (otherPlayerPhotonViews[i].Equals(player))
            {
                inGameUI_otherPlayerZone.RemovePlayerEntry(player);
                otherPlayerPhotonViews.RemoveAt(i);
                return;
            }
        }
    }

    public void AddOtherPlayerPhotonView(PhotonView otherPlayer)
    {
        if (playerPhotonView)
        {
            if (!playerPhotonView.Equals(otherPlayer))
            {
                otherPlayerPhotonViews.Add(otherPlayer);
                inGameUI_otherPlayerZone.AddPlayerEntry(otherPlayer);
            }
        }
        else
        {
            otherPlayerPhotonViews.Add(otherPlayer);
            inGameUI_otherPlayerZone.AddPlayerEntry(otherPlayer);
        }
    }

    void ModifyAggroUIValue(Dictionary<int, float> playerAggroDictionary)
    {
        foreach(KeyValuePair<int, float> playerAggroPair in playerAggroDictionary)
        {
            if (playerAggroPair.Key.Equals(playerPhotonView.ViewID))
            {
                inGameUI_PlayerAggroBar.ModifyAggro(playerAggroPair.Value);
            }
            else
            {
                for(int i = 0; i < otherPlayerPhotonViews.Count; i++)
                {
                    if (playerAggroPair.Key.Equals(otherPlayerPhotonViews[i].ViewID))
                    {
                        inGameUI_otherPlayerZone.ModifyPlayerAggroValue(otherPlayerPhotonViews[i], playerAggroPair.Value);
                        break;
                    }
                }
            }
        }
    }
}
