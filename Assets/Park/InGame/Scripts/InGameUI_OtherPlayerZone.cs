using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI_OtherPlayerZone : SceneUI
{
    [SerializeField] InGameUI_PlayerAggroEntry playerAggroEntry;
    [SerializeField] Dictionary<PhotonView, InGameUI_PlayerAggroEntry> playerAggroEntryDictionary;
    [SerializeField] Animator uiAnimator;

    public override void Initialize()
    {
        base.Initialize();
        StartCoroutine(ShowAndHideRotuine());
    }

    IEnumerator ShowAndHideRotuine()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                uiAnimator.SetTrigger("OnTab");
            }
            yield return null;
        }
    }

    public void AddPlayerEntry(PhotonView playerPhotonView)
    {
        InGameUI_PlayerAggroEntry entry = GameManager.UI.ShowSceneUI(playerAggroEntry, transform);
        entry.SetTargetPlayerPhotonView(playerPhotonView);
        playerAggroEntryDictionary.Add(playerPhotonView, entry);
    }

    public void RemovePlayerEntry(PhotonView playerPhotonView)
    {
        foreach(KeyValuePair<PhotonView, InGameUI_PlayerAggroEntry> keyValuePair in playerAggroEntryDictionary)
        {
            if(keyValuePair.Key.Equals(playerPhotonView))
            {
                GameManager.Resource.Destroy(keyValuePair.Value);
                playerAggroEntryDictionary.Remove(keyValuePair.Key);
                return;
            }
        }
    }

    public void ModifyPlayerAggroValue(PhotonView playerPhotonView, float aggroValue)
    {
        foreach(KeyValuePair<PhotonView, InGameUI_PlayerAggroEntry> photonViewAggroEntryPair in playerAggroEntryDictionary)
        {
            if (photonViewAggroEntryPair.Key.Equals(playerPhotonView))
            {
                photonViewAggroEntryPair.Value.ModifiyAggro(aggroValue);
                return;
            }
        }
    }
}
