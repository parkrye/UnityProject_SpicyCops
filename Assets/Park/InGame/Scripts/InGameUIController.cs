using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] public InGameManager inGameManager;

    [SerializeField] PhotonView playerPhotonView;
    [SerializeField] List<PhotonView> otherPlayerPhotonViews;
    [SerializeField] GameObject optionUI;

    public PhotonView PlayerPhotonView {  get { return playerPhotonView; } }
    public List<PhotonView> OtherPlayerPhotonViews { get { return otherPlayerPhotonViews;} }

    [SerializeField] InGameUI_PlayerDataBar inGameUI_PlayerAggroBar;
    [SerializeField] InGameUI_OtherPlayerZone inGameUI_otherPlayerZone;
    [SerializeField] InGameUI_TimeSlider inGameUI_TimeSlider;
    [SerializeField] InGameUI_EndUI inGameUI_EndUI;
    [SerializeField] InGameUI_ItemIcon inGameUI_ItemIcon;
    [SerializeField] GameObject playingUI;
    [SerializeField] GameObject effectUI;
    [SerializeField] bool isPlaying;
    public bool IsPlaying { get { return isPlaying; } }

    public void Initialize()
    {
        inGameUI_PlayerAggroBar.Initialize();
        inGameUI_otherPlayerZone.Initialize();
        inGameManager.AddPlayerAggroEventListener(PlayerAggroValueModified);
        inGameManager.AddPlayerAliveEventListener(PlayerAliveValueModified);
        optionUI.SetActive(false);
    }

    public void StartTimer()
    {
        inGameUI_TimeSlider.Initialize();
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

    void PlayerAggroValueModified(Dictionary<int, float> playerAggroDictionary)
    {
        foreach(KeyValuePair<int, float> playerAggroPair in playerAggroDictionary)
        {
            if (playerAggroPair.Key.Equals(playerPhotonView.ViewID))
            {
                inGameUI_PlayerAggroBar.ModifyAggroUI(playerAggroPair.Value);
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

    void PlayerAliveValueModified(Dictionary<int, bool> playerAliveDictionary)
    {
        foreach(KeyValuePair<int, bool> playerAlivePair in playerAliveDictionary)
        {
            if (playerAlivePair.Key.Equals(playerPhotonView.ViewID))
            {
                inGameUI_PlayerAggroBar.CheckAlive(playerAlivePair.Value);
            }
            else
            {
                for(int i = 0; i < otherPlayerPhotonViews.Count; i++)
                {
                    if (playerAlivePair.Key.Equals(otherPlayerPhotonViews[i].ViewID))
                    {
                        inGameUI_otherPlayerZone.CheckPlayerAlived(otherPlayerPhotonViews[i], playerAlivePair.Value);
                        break;
                    }
                }
            }
        }
    }

    public void EndGameUI()
    {
        isPlaying = false;
        playingUI.SetActive(false);
        inGameUI_EndUI.gameObject.SetActive(true);
    }

    public void SettingItemIcon(int itemNum)
    {
        if(itemNum < 0)
        {
            inGameUI_ItemIcon.SettingIconImage();
        }
        else
        {
            inGameUI_ItemIcon.SettingIconImage(inGameManager.ItemManager.itemList[itemNum].ItemIcon);
        }
    }
    
    public void DrawEffectOnUI()
    {
        StartCoroutine(EffectRoutine());
    }

    IEnumerator EffectRoutine()
    {
        effectUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        effectUI.SetActive(false);
    }

    void OnESC()
    {
        if (isPlaying && !optionUI.activeSelf)
        {
            optionUI.SetActive(true);
        }
    }

    void OnTab()
    {
        inGameUI_otherPlayerZone.OnTab();
    }
}
