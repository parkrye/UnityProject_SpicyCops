using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoes", menuName = "Item/Util/Shoes")]
public class Shoes : UtilItem
{
    protected override void GetUtilEffect(int viewId)
    {
        // 코루틴 실행 요청
    }
    public override IEnumerator Corutine(int viewId)
    {
        PhotonView view = PhotonView.Find(viewId);
        PlayerMover mover = view.gameObject.GetComponent<PlayerMover>();
        float s = mover.moveSpeed * 0.2f;
        // 속도 증가
        mover.moveSpeed += s;
        yield return new WaitForSeconds(5);
        // 원래대로
        mover.moveSpeed -= s;
    }
}
