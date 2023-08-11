using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseItem : MonoBehaviourPun
{
    [SerializeField] Transform ProjectilePos;
    [SerializeField] ItemManager itemManager;
    [SerializeField] List<Transform> hammers;

    Animator anim;
    private void Awake()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        for(int i = 0; i < hammers.Count; i++)
            hammers[i].gameObject.SetActive(false);

        anim = GetComponent<Animator>();
        myItem = -1;
    }
    private int myItem;
    public int MyItem { get { return myItem; } set { myItem = value; } }
    private void OnItem()
    {
        Debug.Log("사용시도");
        if (MyItem < 0)
            return;
        if (itemManager.itemList[myItem].WeaponType == Define.WeaponType.Projectile)
            itemManager.photonView.RPC("RequestUseItem", RpcTarget.MasterClient, ProjectilePos.position, ProjectilePos.rotation, myItem, photonView.ViewID);
        else
        {
            itemManager.photonView.RPC("RequestUseItem", RpcTarget.MasterClient, transform.position, transform.rotation, myItem, photonView.ViewID);
        }

        if (itemManager.itemList[myItem].WeaponType == Define.WeaponType.Melee)
        {

            //hammer.transform.position = new Vector3(0, 0.008f, 0f);
            //hammer.transform.localEulerAngles = new Vector3(0, 90, 0);

            StartCoroutine(HammerAnimRoutine());
        }
        MyItem = -1;
        itemManager.gameManager.SetItemUI(myItem);
    }

    public IEnumerator HammerRoutine()
    {
        for (int i = 0; i < hammers.Count; i++)
        {
            hammers[i].gameObject.SetActive(true);

        }
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < hammers.Count; i++)
            hammers[i].gameObject.SetActive(false);
    }
    public IEnumerator HammerAnimRoutine()
    {
        anim.SetBool("IsHammer", true);
        StartCoroutine(HammerRoutine());
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("IsHammer", false);
    }
        
}
