using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [SerializeField] protected string itemName;
    [SerializeField] protected Sprite itemIcon;
    [SerializeField] protected int itemIndex;
    [SerializeField] protected Define.WeaponType weaponType;
    [SerializeField] protected List<AudioClip> audioClipList;

    public InGameManager gameManager;

    public string ItemName { get { return itemName; } }
    public Sprite ItemIcon { get { return itemIcon; } }
    public int ItemIndex { get { return itemIndex; } }
    public Define.WeaponType WeaponType { get { return weaponType; } }
    public IEnumerator PlaySFX(Vector3 pos)
    {
        if (audioClipList.Count < 1)
            yield break;
        foreach (AudioClip clip in audioClipList)
        {
            AudioSource source = GameManager.Resource.Instantiate<AudioSource>("AudioSource", pos, Quaternion.identity);
            source.PlayOneShot(clip);
            GameManager.Resource.Destroy(source, 2f);
            yield return new WaitForSeconds(0.25f);
        }
    }
    public virtual void UseItem(Vector3 pos, Quaternion rot, float lag, int viewId, Player sender)
    {

    }
}