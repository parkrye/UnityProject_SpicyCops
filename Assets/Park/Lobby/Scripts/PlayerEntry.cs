using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntry : MonoBehaviour
{
	[SerializeField] public Player player;
	[SerializeField] TMP_Text playerName;
	[SerializeField] TMP_Text playerReady;
	[SerializeField] public Button playerReadyButton, playerNameButton;
    [SerializeField] Button leftAvatarButton, rightAvatarButton, leftColorButton, rightColorButton;

    [SerializeField] int avatarNum, avatarColorNum;
    [SerializeField] Camera avatarCamera;
    [SerializeField] RenderTexture avatarTexture;
    [SerializeField] RawImage avatarImage;
    [SerializeField] CharacterSkinManager[] characterSkinManagers;

    public int ownerId;

	public void Initailize(Player _player, int id, string name, Camera _avatarCamera, RenderTexture _avatarTexture, GameObject avatarRoot)
    {
        player = _player;
        ownerId = id;
        if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {
            playerReadyButton.gameObject.SetActive(false);
            leftAvatarButton.gameObject.SetActive(false);
            rightAvatarButton.gameObject.SetActive(false);
            leftColorButton.gameObject.SetActive(false);
            rightColorButton.gameObject.SetActive(false);
        }
        playerName.text = name;
        playerReady.text = "";
        avatarCamera = _avatarCamera;
        avatarTexture = _avatarTexture;
        avatarImage.texture = avatarTexture;
        characterSkinManagers = avatarRoot.GetComponentsInChildren<CharacterSkinManager>();
        characterSkinManagers[avatarNum].SettingColor(avatarColorNum);
        avatarCamera.transform.position = new Vector3(avatarNum * 5f, avatarCamera.transform.position.y, avatarCamera.transform.position.z);
        StopAllCoroutines();
        StartCoroutine(URoutine());
    }

	IEnumerator URoutine()
	{
		while (true)
		{
			yield return null;
			if(Input.GetKeyDown(KeyCode.F5))
			{
				OnReadyButtonClicked();
            }
		}
	}

	public void SetPlayerReady(bool ready)
	{
		playerReady.text = ready ? "Ready" : "";
	}

	public void OnReadyButtonClicked()
	{
		bool isPlayerReady = !CustomProperty.GetReady(player);
        CustomProperty.SetReady(player, isPlayerReady);

		SetPlayerReady(isPlayerReady);
    }

    public void ChangeCustomProperty(ExitGames.Client.Photon.Hashtable property)
    {
        if (property.TryGetValue(GameData.PLAYER_READY, out object readyValue))
        {
            bool ready = (bool)readyValue;
            playerReady.text = ready ? "Ready" : "";
        }
        else
        {
            playerReady.text = "";
        }

        if (property.TryGetValue(GameData.PLAYER_AVATAR, out object avatarValue))
        {
            avatarNum = (int)avatarValue;
            avatarCamera.transform.position = new Vector3(avatarNum * 5f, avatarCamera.transform.position.y, avatarCamera.transform.position.z);
        }

        if (property.TryGetValue(GameData.PLAYER_COLOR, out object colorValue))
        {
            avatarColorNum = (int)colorValue;
            characterSkinManagers[avatarNum].SettingColor(avatarColorNum);
        }
    }

    public void OnAvatarButtonClicked(bool isLeft)
    {
        if (isLeft)
        {
            avatarNum--;
            if (avatarNum < 0)
                avatarNum = GameData.AVATAR.Count - 1;
            while (!GameData.userData.avaters[GameData.AVATAR[avatarNum]])
            {
                avatarNum--;
                if (avatarNum < 0)
                    avatarNum = GameData.AVATAR.Count - 1;
            }
        }
        else
        {
            avatarNum++;
            if (avatarNum > GameData.AVATAR.Count - 1)
                avatarNum = 0;
            while (!GameData.userData.avaters[GameData.AVATAR[avatarNum]])
            {
                avatarNum++;
                if (avatarNum > GameData.AVATAR.Count - 1)
                    avatarNum = 0;
            }
        }
        player.SetAvatarNumber(avatarNum);
        avatarCamera.transform.position = new Vector3(avatarNum * 5f, avatarCamera.transform.position.y, avatarCamera.transform.position.z);
    }

    public void OnColorButtonClicked(bool isLeft)
    {
        if (isLeft)
        {
            avatarColorNum--;
            if(avatarColorNum < 0)
            {
                avatarColorNum = GameData.AVATAR_COLOR_COUNT - 1;
            }
        }
        else
        {
            avatarColorNum++;
            if (avatarColorNum > GameData.AVATAR_COLOR_COUNT - 1)
            {
                avatarColorNum = 0;
            }
        }
        player.SetAvatarColor(avatarColorNum);
        characterSkinManagers[avatarNum].SettingColor(avatarColorNum);
    }
}
