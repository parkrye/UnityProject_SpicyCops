using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI_OtherPlayerZone : SceneUI
{
    [SerializeField] InGameUI_PlayerDataEntry playerAggroEntry;
    [SerializeField] Dictionary<PhotonView, InGameUI_PlayerDataEntry> playerDataEntryDictionary;
    [SerializeField] Animator uiAnimator;

    public override void Initialize()
    {
        base.Initialize();
        playerDataEntryDictionary = new();
    }

    public void AddPlayerEntry(PhotonView playerPhotonView)
    {
        InGameUI_PlayerDataEntry entry = GameManager.UI.ShowSceneUI(playerAggroEntry, transform);
        entry.SetTargetPlayerPhotonView(playerPhotonView);
        playerDataEntryDictionary.Add(playerPhotonView, entry);
    }

    public void RemovePlayerEntry(PhotonView playerPhotonView)
    {
        foreach(KeyValuePair<PhotonView, InGameUI_PlayerDataEntry> keyValuePair in playerDataEntryDictionary)
        {
            if(keyValuePair.Key.Equals(playerPhotonView))
            {
                GameManager.Resource.Destroy(keyValuePair.Value);
                playerDataEntryDictionary.Remove(keyValuePair.Key);
                return;
            }
        }
    }

    public void ModifyPlayerAggroValue(PhotonView playerPhotonView, float aggroValue)
    {
        foreach(KeyValuePair<PhotonView, InGameUI_PlayerDataEntry> photonViewAggroEntryPair in playerDataEntryDictionary)
        {
            if (photonViewAggroEntryPair.Key.Equals(playerPhotonView))
            {
                photonViewAggroEntryPair.Value.ModifiyAggro(aggroValue);
                return;
            }
        }
    }

    public void CheckPlayerAlived(PhotonView playerPhotonView, bool alive)
    {
        foreach (KeyValuePair<PhotonView, InGameUI_PlayerDataEntry> photonViewAggroEntryPair in playerDataEntryDictionary)
        {
            if (photonViewAggroEntryPair.Key.Equals(playerPhotonView))
            {
                photonViewAggroEntryPair.Value.CheckAlive(alive);
                return;
            }
        }
    }

    public void OnTab()
    {
        uiAnimator.SetTrigger("OnTab");
    }
}
