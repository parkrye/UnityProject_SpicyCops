using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{
	static string playerID = null;

	[SerializeField] TMP_InputField idInputField;

    void Awake()
    {
		GameData.accounts = CSV_RW.ReadAccountsCSV();
    }

    void OnEnable()
	{
		idInputField.text = "";
	}

	public void OnLoginButtonClicked()
	{
		playerID = idInputField.text;

        if (playerID == "")
		{
            playerID = "User";
            GameData.userData = new UserData(playerID);
            GameData.userData.InitializeUserData();
			GameData.userData.avaters[GameData.AVATAR[0]] = true;
        }
		else
		{
			if(GameData.accounts.ContainsKey(playerID))
			{
				GameData.userData = new UserData(playerID, GameData.accounts[playerID].coin);
				GameData.userData.avaters = GameData.accounts[playerID].avaters;
            }
			else
            {
                GameData.userData = new UserData(playerID);
                GameData.userData.InitializeUserData();
                GameData.userData.avaters[GameData.AVATAR[0]] = true;
                GameData.accounts.Add(playerID, GameData.userData);
				CSV_RW.WriteAccountsCSV();
            }
        }

        ExitGames.Client.Photon.Hashtable props = new()
        {
            { GameData.PLAYER_READY, false },
            { GameData.PLAYER_LOAD, false },
			{ GameData.PLAYER_AVATAR, 0 },
			{ GameData.PLAYER_COLOR, 0 },
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        PhotonNetwork.LocalPlayer.NickName = playerID;
		PhotonNetwork.ConnectUsingSettings();
	}

	public void OnQuitButtonClicked()
	{
		Application.Quit();
	}
}
