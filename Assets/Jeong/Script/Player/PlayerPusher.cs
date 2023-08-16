using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerPusher : MonoBehaviourPun
{
    [SerializeField] private bool debug;
    [SerializeField] private float pushForce; // �̴� ��
    [SerializeField] private float pushRange; // �� �� �ִ� ����

    // ******************* �Ʒ��� �������� �޾ƿ� �׸�*********************
    [SerializeField] private float pushDuration; // �̴� �ð�
    [SerializeField] private float pushCooltime; // �б� ��Ÿ��

    private float pushingStartTime; // �б� ���� �ð�
    private bool canPush = true; // �б� �������� ����
    // ******************************************************************

    [SerializeField] public bool isPushing = false;

    private PlayerInput playerInput;
    private Animator anim;
    public UnityEvent PushCoolEvent;


    [SerializeField] private GameObject currentPullTarget;
    [SerializeField] private GameObject targetPlayer;

    private void Start()
    {
        anim = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();

        if (!photonView.IsMine)
            Destroy(playerInput);
    }
    private void OnPush(InputValue value)
    {
        
        // zŰ�� ������ 
        if (value.isPressed && canPush)
        {
            if (currentPullTarget != null)
            {
                isPushing = true;
                canPush = false;
                pushingStartTime = Time.time;
                FindTargetPlayer();

                PushTarget();
                StartCoroutine(PushCooldown());
            }
        }
            
    }

    private void PushTarget()
    {
        // Ÿ���� ã���� �δ�.
        if (isPushing && targetPlayer != null)
        {
            Push(targetPlayer);
        }
    }


    private IEnumerator PushCooldown()
    {
        PushCoolEvent?.Invoke();
        yield return new WaitForSeconds(GameData.PushCoolTime);
        canPush = true; // ��Ÿ�� ���� �� �ٽ� ���� �� �ֵ��� ����
        currentPullTarget = null; // ��Ÿ���� ������ PullTarget �ʱ�ȭ
    }

    [PunRPC]
    private void PushAnim()
    {
        //Debug.LogError($"{PhotonNetwork.LocalPlayer.ActorNumber} : Push Anim Played");
        anim.SetTrigger("IsPushed");
    }

    private void FindTargetPlayer() // ��ƴ�� Player Ž��
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pushRange);

        // ���� ����� Player���� �Ÿ��� �ִ밪���� �ʱ�ȭ�ϰ�, Player�� GameObject�� null�� �ʱ�ȭ�Ѵ�.
        float closestDistance = float.MaxValue;
        GameObject closestPlayer = null;

        // �������� Collider�� ��� Ȯ���ϴ� �۾�
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.gameObject.CompareTag("Player"))
            {
                CapsuleCollider capsuleCollider = collider.gameObject.GetComponent<CapsuleCollider>();
                if (capsuleCollider != null)
                {
                    // ���� Player�� �ٸ� Player ������ �Ÿ� ���
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    // �Ÿ��� �� ����� Player�� ã����
                    if (distance < closestDistance)
                    {
                        // �ִ밪���� �ʱ�ȭ�ߴ� ���� �����Ÿ��� ����ã�� �Ÿ��� �����ϰ�
                        closestDistance = distance;
                        // ���������� null�� ����ִ� Player�� Gameobject�� ���� �����Ѵ�.
                        closestPlayer = collider.gameObject;
                    }
                }
            }
        }
        // ���� ����� Player�� Ž���ϰ� targetPlayer ������ �����Ѵ�.
        targetPlayer = closestPlayer;
    }

    // �б�
    public void Push(GameObject player)
    {
        //Debug.LogError($"{PhotonNetwork.LocalPlayer.ActorNumber} : Push Anim");
        photonView.RPC("PushAnim", RpcTarget.AllViaServer);
        Vector3 directionToTarget = (player.transform.position - transform.position).normalized * pushForce;
        PlayerMover mover = player.GetComponent<PlayerMover>();
        mover.photonView.RPC("mePushing", RpcTarget.AllViaServer, directionToTarget);
    }

    public void SetPullTarget(GameObject target)
    {
        currentPullTarget = target;
    }

    public void ClearPullTarget()
    {
        currentPullTarget = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (!debug)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pushRange);
    }
}
