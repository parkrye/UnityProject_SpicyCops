using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerPuller : MonoBehaviourPun
{
    [SerializeField] private bool debug; // GizmoȮ�ο�

    [SerializeField] private float pullForce = 1f; // ��ƴ��� ��
    [SerializeField] private float pullRange; // ��ƴ��� ����

    // ******************* �Ʒ��� �������� �޾ƿ� �׸�*********************
    [SerializeField] private float pullDuration = 2f; // ��� ���� �ð�
    [SerializeField] private float pullCooltime = 5f; // ��� ��Ÿ��

    private float pullingStartTime; // ��� ���� �ð�
    private bool canPull = true; // ��� �������� ����
                                 // ******************************************************************
    private IEnumerator pullCooltimeCoroutine;

    [SerializeField] public bool isPulling = false;

    [SerializeField] private GameObject currentPullTarget;
    [SerializeField] private GameObject targetPlayer;
    public UnityEvent PullCoolEvent;


    private PlayerInput playerInput;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();

        if (!photonView.IsMine)
            Destroy(playerInput);
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        if (isPulling)
        {
            // ���� �ð��� ���ӽð��� �ʰ��ϸ� ��� ���� ����
            if (Time.time - pullingStartTime > pullDuration)
            {
                isPulling = false;
                photonView.RPC("ChangePullState", RpcTarget.AllViaServer, false);
                targetPlayer = null;
            }

            else
            {
                PullTarget();
            }
        }
    }

    private void OnPull(InputValue value)
    {
        if (!photonView.IsMine)
            return;
         //Debug.LogError($" isPressed : {value.isPressed}, canPull : {canPull}");
        if (value.isPressed && canPull)
        {

            if (currentPullTarget != null)
            {
                isPulling = true;
                pullingStartTime = Time.time;
                FindTargetPlayer();
                photonView.RPC("ChangePullState", RpcTarget.AllViaServer, true);
                canPull = false;
            }
        }
        else
        {
            PullingEnd();

            // PullCooldown �ڷ�ƾ ����
        }
    }

    [PunRPC]
    public void ChangePullState(bool state)
    {
        anim.SetBool("IsPulled", state);

    }

    public void PullingEnd()
    {
        //Debug.LogError("PullingEnd");
        currentPullTarget?.GetComponent<PlayerMover>()?.photonView.RPC("mePullingFinish", RpcTarget.AllViaServer);
        isPulling = false;
        photonView.RPC("ChangePullState", RpcTarget.AllViaServer, false);
        targetPlayer = null;
        StartCoroutine(PullCooldown());
        currentPullTarget = null; // ��Ÿ���� ������ PullTarget �ʱ�ȭ
    }

    private IEnumerator PullCooldown()
    {
        PullCoolEvent?.Invoke();
        yield return new WaitForSeconds(pullCooltime);
        canPull = true; // ��Ÿ�� ���� �� �ٽ� ���� �� �ֵ��� ����
    }

    private void PullTarget()
    {
        //Debug.LogError($"PullTarget {isPulling}, {targetPlayer}");
        // Ÿ���� ã���� ��ƴ���.
        if (isPulling && targetPlayer != null)
        {
            Pull(targetPlayer);
        }
    }

    private void FindTargetPlayer() // ��ƴ�� Player Ž��
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRange);

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

    // ��ƴ��� 
    private void Pull(GameObject player)
    {
        //Debug.LogError("Pull ȣ��");
        PlayerMover mover = player.GetComponent<PlayerMover>();
        mover.RequestPullStart(photonView.ViewID);

        // ��ƴ��� Player�� �ٶ󺸵��� ȸ����Ų��.
        transform.LookAt(player.transform.position, Vector3.up);
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

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pullRange);
    }
}