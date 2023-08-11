using Photon.Pun;
using UnityEngine;

public class ItemSpot : MonoBehaviourPun, IInteractable
{
    public InGameManager manager;
    public int itemSpotIndex;
    Animator animator;
    bool isActive;
    int playerId;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Init()
    {
        isActive = false;
        if(PhotonNetwork.IsMasterClient)
            animator.SetTrigger("Use");
    }

    public void Interact(PlayerInteraction player)
    {
        PlayerUseItem useItem = player.gameObject.GetComponent<PlayerUseItem>();
        if (!isActive || useItem.MyItem > -1)
            return;
        PhotonView view = player.gameObject.GetComponent<PhotonView>();
        playerId = view.ViewID;
        photonView.RPC("RequestGiveRandomItem", RpcTarget.MasterClient, view.ViewID, itemSpotIndex);
    }
    [PunRPC]
    public void RequestGiveRandomItem(int playerId, int itemSpotIndex)
    {
        int randNum = Random.Range(0, (int)Define.ItemIndex.Count);
        // int randNum = (int)Define.ItemIndex.Hammer;
        photonView.RPC("ResultGiveRandomItem", RpcTarget.AllViaServer, playerId, randNum);
    }
    [PunRPC]
    public void ResultGiveRandomItem(int viewId, int index)
    {
        if (!isActive)
            return;
        isActive = false;
        animator.SetTrigger("Use");
        if (viewId != playerId)
            return;
        // 플레이어 보유 아이템을 인덱스로 설정
        PhotonView view = PhotonView.Find(viewId);
        PlayerUseItem useItem = view.gameObject.GetComponent<PlayerUseItem>();
        useItem.MyItem = index;
        manager.SetItemUI(index);
        Debug.Log((Define.ItemIndex)index);
        playerId = -1;
    }
    public void RegenItem()
    {
        isActive = true;
    }
}
