using Photon.Pun;
using UnityEngine;

public class PlayerUseItem : MonoBehaviourPun
{
    [SerializeField] Transform ProjectilePos;
    [SerializeField] ItemManager itemManager;
    private void Awake()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
    }
    private int myItem;
    public int MyItem { get { return myItem; } set { myItem = value; } }
    private void OnItem()
    {
        Debug.Log("사용시도");
        if (MyItem < 0)
            return;
        if (itemManager.itemList[myItem].WeaponType == Define.WeaponType.Projectile)
            itemManager.photonView.RPC("RequestUseItem", RpcTarget.MasterClient, ProjectilePos.position, ProjectilePos.rotation, myItem);
        else
        {
            itemManager.photonView.RPC("RequestUseItem", RpcTarget.MasterClient, transform.position, transform.rotation, myItem);
        }
        MyItem = -1;
    }
  
}
