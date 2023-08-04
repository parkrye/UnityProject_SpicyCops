using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class RoomPanel : SceneUI
{
	[SerializeField] RectTransform playerContent;
	[SerializeField] PlayerEntry playerEntryPrefab;

	[SerializeField] List<PlayerEntry> playerEntryList;
	[SerializeField] RoomInfo roomInfo;
    [SerializeField] GameObject roomPanel, shopPanel;

    [SerializeField] Dictionary<Player, int> avatarDictionary;
    [SerializeField] Camera[] avatarCameras;
    [SerializeField] RenderTexture[] avatarTextures;
    [SerializeField] GameObject[] avatarRoots;

    protected override void Awake()
	{
        base.Awake();
		playerEntryList = new List<PlayerEntry>();
        avatarDictionary = new Dictionary<Player, int>();
    }

    private void OnEnable()
    {
        avatarDictionary = new Dictionary<Player, int>();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
            avatarDictionary.Add(player, avatarDictionary.Count);
            Debug.Log($"{player} : {avatarDictionary[player]}");
            entry.Initailize(player, player.ActorNumber, player.NickName, avatarCameras[avatarDictionary[player]], avatarTextures[avatarDictionary[player]], avatarRoots[avatarDictionary[player]]);
            playerEntryList.Add(entry);
            entry.playerNameButton.onClick.AddListener(() => { OnSwitchMasterClient(player); });
        }

        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);

        AllPlayerReadyCheck();
        PhotonNetwork.AutomaticallySyncScene = true;
        roomPanel.SetActive(true);
        shopPanel.SetActive(false);
    }

    private void OnDisable()
    {
        foreach (PlayerEntry entry in playerEntryList)
        {
            Destroy(entry.gameObject);
        }
        playerEntryList.Clear();

        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public void PlayerEnterRoom(Player newPlayer)
    {
        avatarDictionary.Add(newPlayer, avatarDictionary.Count);

        PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
        entry.Initailize(newPlayer, newPlayer.ActorNumber, newPlayer.NickName, avatarCameras[avatarDictionary[newPlayer]], avatarTextures[avatarDictionary[newPlayer]], avatarRoots[avatarDictionary[newPlayer]]);
        entry.playerNameButton.onClick.AddListener(() => { OnSwitchMasterClient(newPlayer); });
        playerEntryList.Add(entry);
        AllPlayerReadyCheck();

        UpdateRoomState();
    }

    public void PlayerLeftRoom(Player leftPlayer)
    {
        avatarDictionary.Remove(leftPlayer);
        for (int i = 0; i < playerEntryList.Count; i++)
        {
            if (playerEntryList[i].player.Equals(leftPlayer))
            {
                Destroy(playerEntryList[i].gameObject);
                playerEntryList.RemoveAt(i);
                break;
            }
        }
        AllPlayerReadyCheck();

        UpdateRoomState();
    }

    public void PlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        for(int i = 0; i < playerEntryList.Count; i++)
        {
            if (playerEntryList[i].player.Equals(targetPlayer))
            {
                playerEntryList[i].ChangeCustomProperty(changedProps);
                break;
            }
        }
        AllPlayerReadyCheck();
    }

    public void MasterClientSwitched(Player newMasterClient)
    {
        AllPlayerReadyCheck();
    }

    public void UpdateRoomState()
	{
		roomInfo = PhotonNetwork.CurrentRoom;

        roomInfo.CustomProperties.TryGetValue(GameData.ROOMTYPE, out object roomType);
        texts["RoomInfoText"].text = $"Room Name:\n  {roomInfo.Name}\n" +
                            $"Room Tpye:\n  {(string)roomType}\n" +
                            $"Max Player:\n  {PhotonNetwork.PlayerList.Length}/{roomInfo.MaxPlayers}";

		if (!PhotonNetwork.IsMasterClient)
			return;

        buttons["StartButton"].gameObject.SetActive(CheckPlayerReady());
	}

	public bool CheckPlayerReady()
	{
		foreach (Player player in PhotonNetwork.PlayerList)
        {
            return CustomProperty.GetReady(player);
        }

		return true;
    }

    void AllPlayerReadyCheck()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            buttons["StartButton"].gameObject.SetActive(false);
            return;
        }

        int readyCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetReady())
                readyCount++;
        }

        if (readyCount == PhotonNetwork.PlayerList.Length)
            buttons["StartButton"].gameObject.SetActive(true);
        else
            buttons["StartButton"].gameObject.SetActive(false);
    }

    public void OnSwitchMasterClient(Player clickedPlayer)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.SetMasterClient(clickedPlayer);
    }

    public void OnStartButtonClicked()
	{
		PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.CurrentRoom.IsVisible = false;

		PhotonNetwork.LoadLevel("InGameScene");
	}

    public void OnLeaveRoomClicked()
	{
		PhotonNetwork.LeaveRoom();
	}

    public void OnShopButtonClicked()
    {
        for(int i = 0; i < playerEntryList.Count; i++)
        {
            if (playerEntryList[i].player.Equals(PhotonNetwork.LocalPlayer))
            {
                playerEntryList[i].player.SetReady(false);
                AllPlayerReadyCheck();

                roomPanel.SetActive(false);
                shopPanel.SetActive(true);
                return;
            }
        }
    }
}
