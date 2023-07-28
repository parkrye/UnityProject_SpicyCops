using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileItem : Item
{
    [SerializeField] float throwPower = 10;
    [SerializeField] float speed = 1.5f;
    [SerializeField] float explosionArea = 3;
    [SerializeField] LayerMask mask;

    Rigidbody rb;
    bool isEnded;

    // public bool IsBounce;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        StartCoroutine(rrr());
    }
    IEnumerator rrr()
    {
        Vector3 targetPos = (transform.position + (Vector3.forward * throwPower));
        Vector3 startPos = transform.position;
        Vector3 upPos = startPos + new Vector3(0, 2, 0);
        float curTime = 0;
        float xSpeed = 0;
        float ySpeed = 0;
        float zSpeed = 0;

        isEnded = false;
        while (!isEnded)
        {
            xSpeed = Mathf.Lerp(startPos.x, targetPos.x, curTime);
            zSpeed = Mathf.Lerp(startPos.z, targetPos.z, curTime);
            ySpeed = Mathf.Lerp(Mathf.Lerp(startPos.y, upPos.y, curTime), Mathf.Lerp(upPos.y, 0, curTime), curTime);
            transform.position = new Vector3(xSpeed, ySpeed, zSpeed);
            curTime += Time.deltaTime * speed;
            yield return null;
        }
        yield return null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        isEnded = true;
        // 만약 자기 자신 혹은 트리거용 콜라이더가 아니라면
        Explosion();
    }
    private void Explosion()
    {
        // 현재위치 기준 이펙트 및 사운드 적용
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionArea, mask);
        foreach (Collider collider in colliders)
        {
            // PlayerController player =  collider.GetComponent 플레이어
            /* if(player != null)
             *     player.speed -= 100; 
             */
            // 아무튼 효과 부여
        }
    }
}
