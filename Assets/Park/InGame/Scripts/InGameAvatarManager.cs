using System.Collections.Generic;
using UnityEngine;

public class InGameAvatarManager : CharacterSkinManager
{
    [SerializeField] List<GameObject> avatars;
    [SerializeField] int avatarNum;
    
    public void Initialize(int _avatar, int _color)
    {
        avatarNum = _avatar;

        for (int i = 0; i < GameData.AVATAR.Count; i++)
        {
            avatars.Add(Resources.Load<GameObject>($"Avatar/{GameData.AVATAR[i]}"));
        }

        for (int i = 0; i < avatars.Count; i++)
        {
            avatars[i].SetActive(i == avatarNum);
        }

        SettingColor(_color);
    }
}
