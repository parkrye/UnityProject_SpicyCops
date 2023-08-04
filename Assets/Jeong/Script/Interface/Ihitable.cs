using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ihitable
{
    public void Dead()
    {
        Debug.Log("Player Dead");
    }
}
