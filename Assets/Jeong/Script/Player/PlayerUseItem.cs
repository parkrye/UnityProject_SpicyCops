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
        Debug.Log("���õ�");
        if (MyItem < 0)
            return;
        if(true)
            itemManager.photonView.RPC("RequestUseItem", RpcTarget.MasterClient, ProjectilePos.position, ProjectilePos.rotation, myItem);
        else
        {
            itemManager.photonView.RPC("RequestUseItem", RpcTarget.MasterClient, transform.position, transform.rotation, myItem);
        }
        MyItem = -1;
    }
  
}
