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
        if (isActive) // && �������� ������ ���� ���� ��
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
        // �÷��̾� ���� �������� �ε����� ����
    }
    public void RegenItem()
    {
        isActive = true;
    }
}
