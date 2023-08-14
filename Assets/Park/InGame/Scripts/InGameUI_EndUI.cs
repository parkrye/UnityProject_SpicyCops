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
            // ���� ������ �÷��̾� view id
            (int rank, int id) pair = inGameUIController.inGameManager.RankStack.Pop();
            string name = "";

            // ��� �÷��̾� ���� �ѹ�, view id ��ųʸ��� ��ȸ�Ͽ�
            foreach(KeyValuePair<int, int> idPair in inGameUIController.inGameManager.PlayerIDDictionary)
            {
                // view id�� ���� ������ �÷��̾� view id���
                if(pair.id == idPair.Value)
                {
                    // �� �̸��� ����ϰ�
                    name = GameData.userData.id;

                    // �߰��� ���� �ѹ��� �ڽ��̶��
                    if (idPair.Key == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        // �ڽ��� ������ �߰��Ѵ�
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
