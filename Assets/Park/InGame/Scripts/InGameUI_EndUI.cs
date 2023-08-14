using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI_EndUI : SceneUI
{
    [SerializeField] InGameUIController inGameUIController;
    [SerializeField] InGameUI_RankEntry inGameUI_RankEntry;
    [SerializeField] Transform rankEntryParent;
    [SerializeField] GameObject winText, loseText;
    [SerializeField] AudioSource winAudio, loseAudio;

    public void Initialize(InGameUIController _inGameUIController)
    {
        base.Initialize();
        inGameUIController = _inGameUIController;
    }

    public void AddRankEntries()
    {
        while(inGameUIController.inGameManager.RankStack.Count > 0)
        {
            // 현재 순위의 플레이어 view id
            (int rank, int id) pair = inGameUIController.inGameManager.RankStack.Pop();
            string name = "";

            // 모든 플레이어 액터 넘버, view id 딕셔너리를 순회하여
            foreach(KeyValuePair<int, int> idPair in inGameUIController.inGameManager.PlayerIDDictionary)
            {
                // view id가 현재 순위의 플레이어 view id라면
                if(pair.id == idPair.Value)
                {
                    // 그 이름을 사용하고
                    name = GameData.userData.id;

                    // 추가로 액터 넘버가 자신이라면
                    if (idPair.Key == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        // 자신의 코인을 추가한다
                        GameData.userData.coin += GameData.Reward[pair.rank];

                        if (pair.rank == 1)
                        {
                            winAudio.Play();
                            loseText.SetActive(false);
                        }
                        else
                        {
                            loseAudio.Play();
                            winText.SetActive(false);
                        }
                        CSV_RW.WriteAccountsCSV();
                    }
                    break;
                }
            }
            AddPlayerRank(pair.rank, name);
        }
    }

    public void AddPlayerRank(int rank, string name)
    {
        //Debug.Log($"InGameUI_EndUI {name}");
        InGameUI_RankEntry rankEntry = GameManager.Resource.Instantiate<InGameUI_RankEntry>("UI/RankEntry", rankEntryParent);
        rankEntry.SetInitializeReady(rank, name);
        //Debug.Log($"Created Object {rankEntry.name}");
    }

    public void OnEndUIButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
        // PhotonNetwork.LoadLevel("LobbyScene");
    }
}
