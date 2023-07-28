using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
	public enum Panel { Login, InConnect, Lobby, Room }

	[SerializeField] Panel curPanel, prevPanel;

	[SerializeField] LoginPanel loginPanel;
	[SerializeField] InConnectPanel inConnectPanel;
	[SerializeField] RoomPanel roomPanel;
	[SerializeField] LobbyPanel lobbyPanel;

	void Start()
	{
		if (PhotonNetwork.IsConnected)
		{
			OnConnectedToMaster();
		}
		else if (PhotonNetwork.InRoom)
		{
			OnJoinedRoom();
		}
		else if (PhotonNetwork.InLobby)
		{
			OnJoinedLobby();
		}
		else
		{
			OnDisconnected(DisconnectCause.None);
		}
	}

    public override void OnEnable()
    {
		base.OnEnable();
		StartCoroutine(RoomUpdateRoutine());
    }

    public override void OnConnectedToMaster()
	{
		if(prevPanel.Equals(Panel.Login))
			SetActivePanel(Panel.InConnect);
    }

	public override void OnDisconnected(DisconnectCause cause)
	{
		SetActivePanel(Panel.Login);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		SetActivePanel(Panel.Lobby);
		AddMessage(string.Format("Create room failed with error({0}) : {1}", returnCode, message));
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		SetActivePanel(Panel.Lobby);
		AddMessage(string.Format("Join room failed with error({0}) : {1}", returnCode, message));
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		SetActivePanel(Panel.Lobby);
		AddMessage(string.Format("Join random room failed with error({0}) : {1}", returnCode, message));
	}

	public override void OnJoinedRoom()
	{
		SetActivePanel(Panel.Room);
		AddMessage("Join room");

		PhotonNetwork.AutomaticallySyncScene = true;
		roomPanel.UpdateRoomState();
    }

	public override void OnLeftRoom()
	{
		SetActivePanel(Panel.Lobby);
        AddMessage("Left room");
        lobbyPanel.OnLobbyCountChanged();
    }

	public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomPanel.PlayerEnterRoom(newPlayer);
    }

	public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        roomPanel.PlayerLeftRoom(otherPlayer);
    }

	public override void OnMasterClientSwitched(Player newMasterClient)
    {
        roomPanel.MasterClientSwitched(newMasterClient);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        roomPanel.PlayerPropertiesUpdate(targetPlayer, changedProps);
    }

    public override void OnJoinedLobby()
	{
        SetActivePanel(Panel.Lobby);
        lobbyPanel.OnLobbyCountChanged();
    }

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		lobbyPanel.UpdateRoomList(roomList);
	}

	public override void OnLeftLobby()
	{
		SetActivePanel(Panel.InConnect);
        lobbyPanel.OnLobbyCountChanged();
    }

	void SetActivePanel(Panel panel)
	{
		prevPanel = curPanel;
		curPanel = panel;

        loginPanel.gameObject?.SetActive(curPanel == Panel.Login);
		inConnectPanel.gameObject?.SetActive(curPanel == Panel.InConnect);
		roomPanel.gameObject?.SetActive(curPanel == Panel.Room);
		lobbyPanel.gameObject?.SetActive(curPanel == Panel.Lobby);
	}

	void AddMessage(string message)
	{
		StatePanel.Instance.AddMessage(message);
	}

	IEnumerator RoomUpdateRoutine()
	{
		while (PhotonNetwork.InLobby)
        {
            lobbyPanel.OnLobbyCountChanged();
            yield return new WaitForSeconds(1f);
		}
	}
}
