using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HpHandler : NetworkBehaviour
{
    [Networked]
    int HP { get; set; }   
    [Networked]
    public bool isDead { get; set; }
    [Networked]
    bool isVulnerable { get; set; }

    bool isInitalized = false;
    const byte startingHp = 5;
    [SerializeField] private GameObject PlayerGameObject;
    [SerializeField] private GameObject deathGameObjectPrefab;

    CharacterMovementHandler characterMovementHandler;

    Hitbox hitbox;

    // Start is called before the first frame update
    void Start()
    {
        isVulnerable = true;
        HP = startingHp;
        isDead = false;
        characterMovementHandler = gameObject.GetComponent<CharacterMovementHandler>();
        hitbox = gameObject.GetComponent<Hitbox>();
    }

    public void OnTakeDamage()
    {
        if(isDead && !isVulnerable) return;
        isVulnerable = false;
        StartCoroutine("InvulnerableTimer");
        HP -= 1;
        print(HP);

        if(HP <= 0)
        {
            OnDeath();
            isDead = true;
        }
    }

    private void OnDeath()
    {
        Instantiate(deathGameObjectPrefab,transform.position, Quaternion.identity,transform.parent = null);

        transform.position = Utils.GetRandomSpawnPoint();
        isDead = false;

        HP = startingHp;
    }


    IEnumerator InvulnerableTimer()
    {
        isVulnerable = false;
        yield return new WaitForSeconds(0.5f);
        isVulnerable = true;
    }
}
