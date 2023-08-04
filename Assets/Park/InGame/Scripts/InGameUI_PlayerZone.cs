using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI_PlayerZone : SceneUI
{
    [SerializeField] InGameUI_PlayerEntry playerEntry;
    [SerializeField] Dictionary<PhotonView, InGameUI_PlayerEntry> playerEntryDictionary;
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
        InGameUI_PlayerEntry entry = GameManager.UI.ShowSceneUI(playerEntry, transform);
        entry.SetPlayerPhotonView(playerPhotonView);
        playerEntryDictionary.Add(playerPhotonView, entry);
    }

    public void RemovePlayerEntry(PhotonView playerPhotonView)
    {
        foreach(KeyValuePair<PhotonView, InGameUI_PlayerEntry> keyValuePair in playerEntryDictionary)
        {
            if(keyValuePair.Key.Equals(playerPhotonView))
            {
                GameManager.Resource.Destroy(keyValuePair.Value);
                playerEntryDictionary.Remove(keyValuePair.Key);
                return;
            }
        }
    }
}
