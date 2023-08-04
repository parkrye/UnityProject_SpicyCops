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
    [SerializeField] InGameUI_PlayerZone inGameUI_PlayerZone;
    [SerializeField] InGameUI_TimeSlider inGameUI_TimeSlider;

    public void Initialize()
    {
        inGameUI_PlayerZone.Initialize();
        inGameUI_TimeSlider.Initialize();
    }

    public void SetPlayerPhotonView(PhotonView player)
    {
        playerPhotonView = player;
        for(int i = 0; i < otherPlayerPhotonViews.Count; i++)
        {
            if (otherPlayerPhotonViews[i].Equals(player))
            {
                inGameUI_PlayerZone.RemovePlayerEntry(player);
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
                inGameUI_PlayerZone.AddPlayerEntry(otherPlayer);
            }
        }
        else
        {
            otherPlayerPhotonViews.Add(otherPlayer);
            inGameUI_PlayerZone.AddPlayerEntry(otherPlayer);
        }
    }
}
