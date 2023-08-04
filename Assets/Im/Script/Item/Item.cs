using Photon.Realtime;
using UnityEngine;

public class Item : ScriptableObject
{
    [SerializeField] protected string itemName;
    [SerializeField] protected Sprite itemIcon;
    [SerializeField] protected int itemIndex;
    [SerializeField] protected Define.WeaponType weaponType;
    public string ItemName { get { return itemName; } }
    public Sprite ItemIcon { get { return itemIcon; } }
    public int ItemIndex { get { return itemIndex; } }
    public Define.WeaponType WeaponType { get { return weaponType; } }

    public virtual void UseItem(Vector3 pos, Quaternion rot, float lag, Player player)
    {

    }
}