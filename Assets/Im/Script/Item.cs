using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected string itemName;
    protected Sprite itemIcon;
    protected int itemIndex;
    public string ItemName { get { return itemName; } }
    public Sprite ItemIcon { get { return itemIcon; } }
    public int ItemIndex { get { return itemIndex; } }

    
}