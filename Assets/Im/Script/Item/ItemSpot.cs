using Photon.Pun;
using UnityEngine;

public class ItemSpot : MonoBehaviourPun, IInteractable
{
    public ItemManager itemManager;
    public int itemSpotIndex;
    [SerializeField] float regenCoolTime;
    Animator animator;
    bool isActive;
    int playerId;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isActive = false;
    }

    public void Interact(PlayerInteraction player)
    {
        PlayerUseItem useItem = player.gameObject.GetComponent<PlayerUseItem>();
        if (!isActive || useItem.MyItem > -1)
            return;
        PhotonView view = player.GetComponent<PhotonView>();
        playerId = view.ViewID;
        itemManager.photonView.RPC("RequestGiveRandomItem", RpcTarget.MasterClient, view.ViewID, itemSpotIndex);
    }
    [PunRPC]
    public void ResultGiveRandomItem(int viewId, int index)
    {
        isActive = false;
        if (viewId != playerId)
            return;
        animator.SetTrigger("Use");
        // 플레이어 보유 아이템을 인덱스로 설정
        PhotonView view = PhotonView.Find(viewId);
        PlayerUseItem useItem = view.gameObject.GetComponent<PlayerUseItem>();
        useItem.MyItem = index;
        Debug.Log((Define.ItemIndex)index);
    }
    public void RegenItem()
    {
        isActive = true;
    }
}
