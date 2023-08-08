using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPuller : MonoBehaviourPun
{
    [SerializeField] private bool debug; // GizmoȮ�ο�

    [SerializeField] private float pullForce; // ��ƴ��� ��
    [SerializeField] private float pullRange; // ��ƴ��� ����

    // ******************* �Ʒ��� �������� �޾ƿ� �׸�*********************
    [SerializeField] private float pullDuration; // ��� ���� �ð�
    [SerializeField] private float pullCooltime; // ��� ��Ÿ��

    private float pullingStartTime; // ��� ���� �ð�
    private bool canPull = true; // ��� �������� ����
    // ******************************************************************

    
    [SerializeField] private bool isPulling = false;

    [SerializeField] private GameObject currentPullTarget;
    [SerializeField] private GameObject targetPlayer;

    private PlayerInput playerInput;
    private Animator anim;

    private void Start()
    {
        playerInput  = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();

        if (!photonView.IsMine)
            Destroy(playerInput);
    }

    private void FixedUpdate()
    {
        if (isPulling)
        {
            // ���� �ð��� ���ӽð��� �ʰ��ϸ� ��� ���� ����
            if (Time.time - pullingStartTime > pullDuration)
            {
                isPulling = false;
                anim.SetBool("IsPulled", false);
                targetPlayer = null;
                // ��Ÿ�� ����
                StartCoroutine(PullCooldown());
            }

            // ��ƴ��� ����
            else
            {
                PullTarget();
            }
        }
        else if (!canPull && Time.time - pullingStartTime < pullCooltime)
        {
            // Debug.Log("Cooltimes: " + Mathf.Max(0, (pullingStartTime + pullCooltime - Time.time)).ToString("0") + " seconds");
        }
    }

  

    private void OnPull(InputValue value)
    {
        // xŰ�� ������ 
        if (value.isPressed && canPull)
        {
            if (currentPullTarget != null)
            {
                isPulling = true;
                pullingStartTime = Time.time;
                FindTargetPlayer();
                anim.SetBool("IsPulled", true);
                canPull = false; // ��� �� ��Ÿ�� ����
            }
        }
        else
        { 
            isPulling = false;
            anim.SetBool("IsPulled", false);
            targetPlayer = null; // ��� ����
        }
    }

    private IEnumerator PullCooldown()
    {
        yield return new WaitForSeconds(pullCooltime);
        canPull = true; // ��Ÿ�� ���� �� �ٽ� ���� �� �ֵ��� ����
        currentPullTarget = null; // ��Ÿ���� ������ PullTarget �ʱ�ȭ
    }

    private void PullTarget()
    {
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
        
        // ���� Player ������Ʈ�� ��ƴ����� Player ������Ʈ ������ ���� Vector�� ��� �� ���̸�ŭ �Ÿ��� ���Ѵ�.
        Vector3 directionToTarget = (player.transform.position - transform.position).normalized;

        // ��ƴ����� Player ������Ʈ�� CharacterController ������Ʈ�� �̿��Ͽ�, ���� ����� pullForce��ŭ ���� ���ؼ� Player�� ��ƴ���.
        player.GetComponent<CharacterController>().Move(directionToTarget * -pullForce);

        // ��ƴ��� Player�� ������ Player�� �ٶ󺸵��� ȸ����Ų��.
        player.transform.LookAt(transform.position, Vector3.up);

        // ��ƴ��� Player�� �ٶ󺸵��� ȸ����Ų��.
        transform.LookAt(player.transform.position, Vector3.up);

        // ��ƴ��� Player�� ������ Player�� �ӵ��� ��������.
        float slowDownFactor = 0.5f;
        player.GetComponent<PlayerMover>().SetMoveSpeed(player.GetComponent<PlayerMover>().moveSpeed * slowDownFactor);
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