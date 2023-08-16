using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTest : MonoBehaviour
{
    private Collider coll;

    private void Awake()
    {
        coll = GetComponent<Collider>();
    }

    public void GetItem()
    {
        Debug.Log("get " + name);

        Destroy(gameObject);
    }
}
