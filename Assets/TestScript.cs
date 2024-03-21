using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            if (Object.HasStateAuthority)
                other.GetComponent<HpHandler>().OnTakeDamage();
    }
}
