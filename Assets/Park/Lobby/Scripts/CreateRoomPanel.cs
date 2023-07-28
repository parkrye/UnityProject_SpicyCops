using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class CreateRoomPanel : MonoBehaviour
{
    [SerializeField] GameObject prevPanel;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField maxPlayerInputField;
    [SerializeField] TMP_InputField passwordInputField;

    public void OnCreateRoomCancelButtonClicked()
    {
        gameObject.SetActive(false);
        prevPanel.gameObject.SetActive(true);
    }

    public void OnCreateRoomConfirmButtonClicked()
    {
        string roomName = roomNameInputField.text;
        if (roomName == "")
            roomName = string.Format("Room{0}", Random.Range(1000, 10000));

        int maxPlayer = maxPlayerInputField.text == "" ? 4 : int.Parse(maxPlayerInputField.text);
        maxPlayer = Mathf.Clamp(maxPlayer, 1, 4);

        string password = passwordInputField.text;

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
