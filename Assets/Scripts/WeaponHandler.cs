using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class WeaponHandler : NetworkBehaviour
{
    [Networked()]
    public bool isFiring { get; private set; }

    public Transform aimPoint;
    public LayerMask collisionLayers;
    public GameObject sword;
    float lastimeFired = 0;


    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData networkInputData))
        {
            if (networkInputData.isFireButtomPressed)
                Fire(networkInputData.aimFowardVector);
        }
    }
    void Fire(Vector3 aimFowardVector)
    {
        if (Time.time - lastimeFired < 0.15f)
            return;

        StartCoroutine("FireEffectCO");

        Runner.LagCompensation.Raycast(aimPoint.transform.position, aimFowardVector, 50, Object.InputAuthority, out var hitinfo, collisionLayers);

        sword.transform.position = aimFowardVector;

        float hitDistance = 100;
        bool isHitOtherPlayer = false;

        if(hitinfo.Distance > 0)
            hitDistance = hitinfo.Distance;

        if(hitinfo.Hitbox != null)
        {
            isHitOtherPlayer = true;

            if (Object.HasStateAuthority)
                hitinfo.Hitbox.transform.root.GetComponent<HpHandler>().OnTakeDamage();

        }


        if (isHitOtherPlayer)
        {
            Debug.DrawRay(aimPoint.position, aimFowardVector * hitDistance, Color.red, 1);

        }
        else
            Debug.DrawRay(aimPoint.position, aimFowardVector * hitDistance, Color.green, 1);

        lastimeFired = Time.time;
    }

    IEnumerator FireEffectCO()
    {
        isFiring = true;
        sword.SetActive(true);
        yield return new WaitForSeconds(1f);
        isFiring = false;
        sword.SetActive(false);
    }

    void OnFireRemote(Vector3 aimFowardVector)
    {

    }
}
