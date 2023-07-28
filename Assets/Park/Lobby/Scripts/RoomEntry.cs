using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RoomEntry : MonoBehaviour
{
	[SerializeField] TMP_Text roomName;
	[SerializeField] TMP_Text currentPlayer;
	[SerializeField] Button joinRoomButton;
	[SerializeField] TMP_Text lockText;
    [SerializeField] RoomInfo roomInfo;
    [SerializeField] EnterPrivateRoomPanel enterPrivateRoomPanel;

	public void Initialize(string name, int currentPlayers, byte maxPlayers, RoomInfo _roomInfo, EnterPrivateRoomPanel _enterPrivateRoomPanel)
	{
		roomName.text = name;
		currentPlayer.text = string.Format("{0} / {1}", currentPlayers, maxPlayers);
		joinRoomButton.interactable = currentPlayers < maxPlayers;
		roomInfo = _roomInfo;
        if (roomInfo != null )
        {
            if (roomInfo.CustomProperties.TryGetValue(GameData.ROOMTYPE, out object lockValue))
            {
                StatePanel.Instance.AddMessage($"Room Type is {(string)lockValue}");
                if (((string)lockValue).Equals(GameData.PRIVATE))
				{
					lockText.text = GameData.PRIVATE;
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
		PhotonNetwork.JoinRoom(roomName.text);
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
