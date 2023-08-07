using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPuller : MonoBehaviour
{
    [SerializeField] private bool debug; // RayȮ�ο�

    [SerializeField] private float pullForce; // ��ƴ��� ��
    [SerializeField] private float pullRange; // ��ƴ��� ����

    // ******************* �Ʒ��� �������� �޾ƿ� �׸�*********************
    [SerializeField] private float pullDuration; // ��� ���� �ð�
    [SerializeField] private float pullCooltime; // ��� ��Ÿ��

    private float pullingStartTime; // ��� ���� �ð�
    private bool canPull = true; // ��� �������� ����
    // ******************************************************************

    [SerializeField] private PlayerMover mover;
    [SerializeField] private bool isPulling = false;

    private GameObject targetPlayer;
    [SerializeField] private GameObject currentPullTarget;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
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
           /* // isPulling�� true�� ����ؼ� Ÿ���� ã�´�.
            // ���� �ð��� ���ӽð��� �ʰ��ϸ� ��� ���� ����
            if (isPulling)
            {
                // PlayerMover Ŭ������ isPulling ���� ����
                mover.SetIsPulling(true);
                FindTargetPlayer();
            }
            else
            {
                // PlayerMover Ŭ������ isPulling ���� ����
                mover.SetIsPulling(false);
            }*/
        }
    }
        else if (!canPull && Time.time - pullingStartTime < pullCooltime)
        {
            Debug.Log("Cooltime: " + Mathf.Max(0, (pullingStartTime + pullCooltime - Time.time)).ToString("0") + " seconds");
        }
    }

  

    private void OnPull(InputValue value)
    {
        
        // xŰ�� ������ 
        if (value.isPressed && canPull)
        {
            if (currentPullTarget != null)
            {
                Debug.Log("OnPull == true");
                isPulling = true;
                pullingStartTime = Time.time;
                anim.SetBool("IsPulled", true);
                canPull = false; // ��� �� ��Ÿ�� ����
            }
        }
        else
        {
            Debug.Log("OnPull == false");
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
        Debug.Log("PullTarget");
        // Ÿ���� ã���� ��ƴ���.
        if (isPulling && targetPlayer != null)
        {
            Pull(targetPlayer);
        }
    }

    private void FindTargetPlayer() // ��ƴ�� Player Ž��
    {
        Debug.Log("FindTargetPlayer");
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRange);

        // ���� ����� Player���� �Ÿ��� �ִ밪���� �ʱ�ȭ�ϰ�, Player�� GameObject�� null�� �ʱ�ȭ�Ѵ�.
        float closestDistance = float.MaxValue;
        GameObject closestPlayer = null;

        // �������� Collider�� ��� Ȯ���ϴ� �۾�
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.gameObject.CompareTag("Player"))
            {
                SphereCollider sphereCollider = collider.gameObject.GetComponent<SphereCollider>();
                if (sphereCollider != null && sphereCollider == currentPullTarget)
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
        Debug.Log("Pull");
        // ���� Player ������Ʈ�� ��ƴ����� Player ������Ʈ ������ ���� Vector�� ��� �� ���̸�ŭ �Ÿ��� ���Ѵ�.
        Vector3 directionToTarget = (player.transform.position - transform.position).normalized;

        // ��ƴ����� Player ������Ʈ�� Rigidbody ������Ʈ�� �̿��Ͽ�, ���� ����� pullForce��ŭ ���� ���ؼ� Player�� ��ƴ���.
        player.GetComponent<Rigidbody>().AddForce(directionToTarget * -pullForce, ForceMode.Force);

        // ��ƴ��� Player�� ������ Player�� �ٶ󺸵��� ȸ����Ų��.
        player.transform.LookAt(transform.position, Vector3.up);

        // ��ƴ��� Player�� �ٶ󺸵��� ȸ����Ų��.
        transform.LookAt(player.transform.position, Vector3.up);

        // ��ƴ��� Player�� ������ Player�� �ӵ��� ��������.
        float slowDownFactor = 0.5f;
        player.GetComponent<PlayerMover>().SetMoveSpeed(player.GetComponent<PlayerMover>().moveSpeed * slowDownFactor);

        // PlayerMover�� targetPlayer�� �����Ѵ�.
        player.GetComponent<PlayerMover>().SetTargetPlayer(gameObject);
    }

    public void SetPullTarget(GameObject target)
    {
        Debug.Log("SetPullTarget");
        currentPullTarget = target;
    }

    public void ClearPullTarget()
    {
        Debug.Log("ClearPullTarget");
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