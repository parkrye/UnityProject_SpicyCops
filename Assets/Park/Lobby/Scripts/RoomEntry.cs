using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
public class RoomEntry : SceneUI
{
    [SerializeField] RoomInfo roomInfo;
    [SerializeField] EnterPrivateRoomPanel enterPrivateRoomPanel;

    public void Initialize(string name, int currentPlayers, byte maxPlayers, RoomInfo _roomInfo, EnterPrivateRoomPanel _enterPrivateRoomPanel)
	{
        texts["RoomNameText"].text = name;
        texts["CurrentPlayerText"].text = string.Format("{0} / {1}", currentPlayers, maxPlayers);
        buttons["JoinButton"].interactable = currentPlayers < maxPlayers;
		roomInfo = _roomInfo;
        if (roomInfo != null )
        {
            if (roomInfo.CustomProperties.TryGetValue(GameData.ROOMTYPE, out object lockValue))
            {
                StatePanel.Instance.AddMessage($"Room Type is {(string)lockValue}");
                if (((string)lockValue).Equals(GameData.PRIVATE))
				{
                    texts["PasswordText"].text = GameData.PRIVATE;
                }
            }
        }
        enterPrivateRoomPanel = _enterPrivateRoomPanel;
	}

	public void OnJoinRoomClicked()
    {
        StatePanel.Instance.AddMessage($"Join to {roomInfo.Name}");
        if (roomInfo.CustomProperties.TryGetValue(GameData.ROOMTYPE, out object lockValue))
        {
            StatePanel.Instance.AddMessage($"Room Type is {(string)lockValue}");
            if (((string)lockValue).Equals(GameData.PRIVATE))
            {
                if (!enterPrivateRoomPanel)
                    return;

                enterPrivateRoomPanel.gameObject.SetActive(true);
                enterPrivateRoomPanel.Initialize(this);

                return;
            }
		}

        PhotonNetwork.LeaveLobby();
		PhotonNetwork.JoinRoom(texts["RoomNameText"].text);
	}

    public bool EnterPrivateRoom()
    {
        string inputPassword = enterPrivateRoomPanel.passwordInputField.text;

        if (roomInfo.CustomProperties.TryGetValue(GameData.ROOMPASSWORD, out object passwordValue))
        {
            string roomPassword = (string)passwordValue;
            StatePanel.Instance.AddMessage($"Password is {roomPassword}");
            if (roomPassword.Length > 0)
            {
                if (!inputPassword.Equals(roomPassword))
                {
                    StatePanel.Instance.AddMessage($"{roomPassword} is Wrong passwordInputField");
                    return false;
                }
            }
        }

        return true;
    }
}
