using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntry : SceneUI
{
	[SerializeField] public Player player;
	[SerializeField] public Button playerNameButton;

    [SerializeField] int avatarNum, avatarColorNum, entryNum;
    [SerializeField] Camera avatarCamera;
    [SerializeField] RenderTexture avatarTexture;
    [SerializeField] RawImage avatarImage;
    [SerializeField] CharacterSkinManager[] characterSkinManagers;
    [SerializeField] Image crownImage;
    [SerializeField] int myNum;
    public int EntryNum { get { return entryNum; } }

	public void Initailize(Player _player, int id, string name, Camera _avatarCamera, RenderTexture _avatarTexture, GameObject avatarRoot, int _numbering)
    {
        player = _player;
        entryNum = id;
        if (PhotonNetwork.LocalPlayer.ActorNumber != player.ActorNumber)
        {
            buttons["PlayerReadyButton"].gameObject.SetActive(false);
            buttons["LeftAvatarButton"].gameObject.SetActive(false);
            buttons["RightAvatarButton"].gameObject.SetActive(false);
            buttons["LefColorButton"].gameObject.SetActive(false);
            buttons["RightColorButton"].gameObject.SetActive(false);
        }
        texts["PlayerNameText"].text = name;
        texts["ReadyText"].text = "";
        avatarCamera = _avatarCamera;
        avatarTexture = _avatarTexture;
        myNum = _numbering;
        avatarImage.texture = avatarTexture;
        characterSkinManagers = avatarRoot.GetComponentsInChildren<CharacterSkinManager>();

        if (CustomProperty.GetReady(player))
        {
            SetPlayerReady(true);
        }

        if (player.CustomProperties.TryGetValue(GameData.PLAYER_AVATAR, out object avatarValue))
        {
            avatarNum = (int)avatarValue;
            avatarCamera.transform.position = new Vector3(avatarNum * 5f, avatarCamera.transform.position.y, avatarCamera.transform.position.z);
        }

        if (player.CustomProperties.TryGetValue(GameData.PLAYER_COLOR, out object colorValue))
        {
            avatarColorNum = (int)colorValue;
            characterSkinManagers[avatarNum].SettingColor(avatarColorNum);
        }

        characterSkinManagers[avatarNum].SettingColor(avatarColorNum);
        avatarCamera.transform.position = new Vector3(avatarNum * 5f, avatarCamera.transform.position.y, avatarCamera.transform.position.z);

        if(PhotonNetwork.MasterClient.ActorNumber == myNum)
            crownImage.enabled = true;
        else
            crownImage.enabled = false;
    }

	public void SetPlayerReady(bool ready)
	{
        texts["ReadyText"].text = ready ? "Ready" : "";
        if (ready)
        {
            GameData.CurrentAvatarNum = avatarNum;
            GameData.CurrentColorNum = avatarColorNum;
        }
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
            texts["ReadyText"].text = ready ? "Ready" : "";
        }
        else
        {
            texts["ReadyText"].text = "";
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

        avatarColorNum = 0;
        player.SetAvatarColor(avatarColorNum);
        characterSkinManagers[avatarNum].SettingColor(avatarColorNum);

        CustomProperty.SetAvatarNumber(player, avatarNum);
    }

    public void OnColorButtonClicked(bool isLeft)
    {
        if (isLeft)
        {
            avatarColorNum--;
            if(avatarColorNum < 0)
                avatarColorNum = GameData.AVATAR_COLOR_COUNT - 1;
        }
        else
        {
            avatarColorNum++;
            if (avatarColorNum > GameData.AVATAR_COLOR_COUNT - 1)
                avatarColorNum = 0;
        }
        player.SetAvatarColor(avatarColorNum);
        characterSkinManagers[avatarNum].SettingColor(avatarColorNum);

        CustomProperty.SetAvatarColor(player, avatarColorNum);
    }

    public void CheckAmIMaster()
    {
        //Debug.LogError($"{myNum}, {PhotonNetwork.LocalPlayer.GetPlayerNumber()}");
        if (PhotonNetwork.MasterClient.ActorNumber == myNum)
            crownImage.enabled = true;
        else
            crownImage.enabled = false;
    }

    void OnF5()
    {
        //Debug.LogError($"{myNum}, {PhotonNetwork.LocalPlayer.GetPlayerNumber()}");
        if(myNum == PhotonNetwork.LocalPlayer.ActorNumber)
            OnReadyButtonClicked();
    }
}
