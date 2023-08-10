using System.Collections.Generic;
using UnityEngine;

public class InGameAvatarManager : CharacterSkinManager
{
    [SerializeField] List<GameObject> avatars;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] int avatarNum;
    
    public void Initialize(int _avatar, int _color)
    {
        avatars = new List<GameObject>();
        avatarNum = _avatar;

        for(int i = 0; i < avatars.Count; i++)
        {
            avatars[i].SetActive(i == avatarNum);
        }

        SettingColor(_color);
    }
}
