using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CreateRoomPanel : SceneUI
{
    [SerializeField] GameObject prevPanel;

    public void OnCreateRoomCancelButtonClicked()
    {
        gameObject.SetActive(false);
        prevPanel.gameObject.SetActive(true);
    }

    public void OnCreateRoomConfirmButtonClicked()
    {
        string roomName = inputFields["RoomNameInputField"].text;
        if (roomName == "")
            roomName = string.Format("Room{0}", Random.Range(1000, 10000));

        int maxPlayer = inputFields["MaxPlayerInputField"].text == "" ? 4 : int.Parse(inputFields["MaxPlayerInputField"].text);
        maxPlayer = Mathf.Clamp(maxPlayer, 1, 4);

        string password = inputFields["PasswordInputField"].text;

        RoomOptions roomOptions = new()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)maxPlayer,
            CustomRoomProperties = new()
            {
                { GameData.ROOMTYPE, password.Length > 0 ? GameData.PRIVATE : GameData.PUBLIC },
                { GameData.ROOMPASSWORD, password.Length > 0 ? password : null }
            },
            CustomRoomPropertiesForLobby = new[]
            {
                GameData.ROOMTYPE,
                GameData.ROOMPASSWORD
            }
        };
        gameObject.SetActive(false);
        prevPanel.gameObject.SetActive(true);

        PhotonNetwork.CreateRoom(roomName, roomOptions, null);
    }
}
