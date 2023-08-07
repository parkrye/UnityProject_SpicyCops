using Photon.Pun;
using UnityEngine;

public class ItemSpot : MonoBehaviourPun, IInteractable
{
    [SerializeField] ItemManager itemManager;
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
        if (isActive) // && �������� ������ ���� ���� ��
        {
            PhotonView view = player.GetComponent<PhotonView>();
            playerId = view.ViewID;
            itemManager.photonView.RPC("RequestGiveRandomItem", RpcTarget.MasterClient, view.ViewID);
        }
    }
    [PunRPC]
    public void ResultGiveRandomItem(int viewId, int index)
    {
        isActive = false;
        if (viewId != playerId)
            return;
        animator.SetTrigger("Use");
        // �÷��̾� ���� �������� �ε����� ����
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
