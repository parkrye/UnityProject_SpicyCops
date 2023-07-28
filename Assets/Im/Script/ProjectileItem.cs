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
        // ���� �ڱ� �ڽ� Ȥ�� Ʈ���ſ� �ݶ��̴��� �ƴ϶��
        Explosion();
    }
    private void Explosion()
    {
        // ������ġ ���� ����Ʈ �� ���� ����
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionArea, mask);
        foreach (Collider collider in colliders)
        {
            // PlayerController player =  collider.GetComponent �÷��̾�
            /* if(player != null)
             *     player.speed -= 100; 
             */
            // �ƹ�ư ȿ�� �ο�
        }
    }
}
