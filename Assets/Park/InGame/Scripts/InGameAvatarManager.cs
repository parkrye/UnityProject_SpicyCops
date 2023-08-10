using System.Collections.Generic;
using UnityEngine;

public class InGameAvatarManager : CharacterSkinManager
{
    [SerializeField] Animator animator;
    [SerializeField] List<GameObject> characters;
    [SerializeField] List<Avatar> avatars;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] int avatarNum;
    
    public void Initialize(int _avatar, int _color)
    {
        avatarNum = _avatar;

        for(int i = 0; i < characters.Count; i++)
        {
            characters[i].SetActive(i == avatarNum);
        }
        animator.avatar = avatars[avatarNum];

        SettingColor(_color);
    }
}
