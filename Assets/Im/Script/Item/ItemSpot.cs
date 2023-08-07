using Photon.Pun;
using UnityEngine;

public class ItemSpot : MonoBehaviourPun
{
    [SerializeField] ItemManager itemManager;
    [SerializeField] float regenCoolTime;
    Animator animator;
    bool isActive;
    PlayerInteraction interactPlayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isActive = false;
    }

    public void Communicate(PlayerInteraction player)
    {
        if (isActive) // && 아이템을 가지고 있지 않을 것
        {
            interactPlayer = player;
            photonView.RPC("RequestGiveRandomItem", RpcTarget.MasterClient, player);
            
        }
    }
    [PunRPC]
    public void ResultGiveRandomItem(PlayerInteraction player, int index)
    {
        isActive = false;
        if (player != interactPlayer)
            return;
        animator.SetTrigger("Use");
        // 플레이어 보유 아이템을 인덱스로 설정
    }
    public void RegenItem()
    {
        isActive = true;
    }
}
