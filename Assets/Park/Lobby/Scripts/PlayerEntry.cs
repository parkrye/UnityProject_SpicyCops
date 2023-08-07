using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntry : SceneUI
{
	[SerializeField] public Player player;
	[SerializeField] public Button playerNameButton;

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
    }

    public override void Initialize()
    {
        throw new System.NotImplementedException();
    }
}
