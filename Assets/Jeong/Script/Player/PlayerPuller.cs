using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPuller : MonoBehaviourPun
{
    [SerializeField] private bool debug; // Gizmo확인용

    [SerializeField] private float pullForce; // 잡아당기는 힘
    [SerializeField] private float pullRange; // 잡아당기는 범위

    // ******************* 아래는 서버에서 받아올 항목*********************
    [SerializeField] private float pullDuration; // 잡기 지속 시간
    [SerializeField] private float pullCooltime; // 잡기 쿨타임

    private float pullingStartTime; // 잡기 시작 시간
    private bool canPull = true; // 잡기 가능한지 여부
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
            // 현재 시간이 지속시간을 초과하면 잡기 상태 해제
            if (Time.time - pullingStartTime > pullDuration)
            {
                isPulling = false;
                anim.SetBool("IsPulled", false);
                targetPlayer = null;
                // 쿨타임 시작
                StartCoroutine(PullCooldown());
            }

            // 잡아당기기 동작
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
        // x키를 누르면 
        if (value.isPressed && canPull)
        {
            if (currentPullTarget != null)
            {
                isPulling = true;
                pullingStartTime = Time.time;
                FindTargetPlayer();
                anim.SetBool("IsPulled", true);
                canPull = false; // 잡기 후 쿨타임 적용
            }
        }
        else
        { 
            isPulling = false;
            anim.SetBool("IsPulled", false);
            targetPlayer = null; // 잡기 해제
        }
    }

    private IEnumerator PullCooldown()
    {
        yield return new WaitForSeconds(pullCooltime);
        canPull = true; // 쿨타임 종료 후 다시 잡을 수 있도록 설정
        currentPullTarget = null; // 쿨타임이 끝나면 PullTarget 초기화
    }

    private void PullTarget()
    {
        // 타겟을 찾으면 잡아당긴다.
        if (isPulling && targetPlayer != null)
        {
            Pull(targetPlayer);
        }
    }

    private void FindTargetPlayer() // 잡아당길 Player 탐색
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRange);

        // 가장 가까운 Player와의 거리를 최대값으로 초기화하고, Player의 GameObject를 null로 초기화한다.
        float closestDistance = float.MaxValue;
        GameObject closestPlayer = null;

        // 범위안의 Collider를 모두 확인하는 작업
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.gameObject.CompareTag("Player"))
            {
                CapsuleCollider capsuleCollider = collider.gameObject.GetComponent<CapsuleCollider>();
                if (capsuleCollider != null)
                {
                    // 현재 Player와 다른 Player 사이의 거리 계산
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    // 거리가 더 가까운 Player를 찾으면
                    if (distance < closestDistance)
                    {
                        // 최대값으로 초기화했던 가장 가까운거리를 새로찾은 거리로 갱신하고
                        closestDistance = distance;
                        // 마찬가지로 null이 들어있던 Player의 Gameobject도 새로 갱신한다.
                        closestPlayer = collider.gameObject;
                    }
                }
            }
        }
        // 가장 가까운 Player을 탐색하고 targetPlayer 변수에 갱신한다.
        targetPlayer = closestPlayer;
    }

    // 잡아당기기 
    private void Pull(GameObject player)
    {
        
        // 현재 Player 오브젝트와 잡아당기려는 Player 오브젝트 사이의 방향 Vector를 계산 후 차이만큼 거리를 구한다.
        Vector3 directionToTarget = (player.transform.position - transform.position).normalized;

        // 잡아당기려는 Player 오브젝트의 CharacterController 컴포넌트를 이용하여, 계산된 방향과 pullForce만큼 힘을 가해서 Player를 잡아당긴다.
        player.GetComponent<CharacterController>().Move(directionToTarget * -pullForce);

        // 잡아당기는 Player가 잡히는 Player를 바라보도록 회전시킨다.
        player.transform.LookAt(transform.position, Vector3.up);

        // 잡아당기는 Player만 바라보도록 회전시킨다.
        transform.LookAt(player.transform.position, Vector3.up);

        // 잡아당기는 Player와 잡히는 Player는 속도가 느려진다.
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