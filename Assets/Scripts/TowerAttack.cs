using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerAttack : MonoBehaviour
{
    public TowerType towerType;

    [Header("Common Settings")]
    public float radius = 10f;
    public float fireDelay = 1f;
    public int damage = 10;

    [Header("Visual Settings")]
    public ParticleSystem radiusParticle;
    public GameObject laserSpawn;
    public GameObject laserPrefab;
    public ParticleSystem shockParticle;

    [Header("Tank Settings")]
    public float aoeRadius = 10f;
    public int aoeDamage = 5;

    [Header("Shock Settings")]
    public float shockTime = 2f;
    public float shockSpeed = 0f;


    private GameObject targetEnemy;
    private bool attacking = false;

    // Max detection of 250 enemies at a time
    private readonly Collider[] hitColliders = new Collider[250];
    private Vector3 targetRotation;
    private readonly Collider[] aoeColliders = new Collider[250];



    public enum TowerType
    {
        Standard,
        Tank,
        Sniper,
        Electric
    }


    private void Awake()
    {
        var main = radiusParticle.main;
        main.startSize = radius * 2;

        var shockMain = shockParticle.main;
        shockMain.startSize = radius * 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Code source: https://docs.unity3d.com/ScriptReference/Physics.OverlapSphereNonAlloc.html
        Array.Clear(hitColliders, 0, hitColliders.Length);
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders);
        float shortestDistance = radius;
        float longestDistance = 0;
        int targetIndex = -1;

        // Choose the target based on distance
        for (int i = 0; i < numColliders; i++)
        {
            float distance = (hitColliders[i].transform.position - transform.position).magnitude;
            
            if (towerType < TowerType.Sniper)
            {
                if (distance < shortestDistance && hitColliders[i].tag == "follow")
                {
                    targetIndex = i;
                    shortestDistance = distance;
                }
            }
            else if (towerType == TowerType.Sniper)
            {
                if (distance > longestDistance && hitColliders[i].tag == "follow")
                {
                    targetIndex = i;
                    longestDistance = distance;
                }
            }
        }

        // Assign the target to be attacked if it exists
        if (targetIndex > -1)
        {
            targetEnemy = hitColliders[targetIndex].gameObject;
            if (!attacking)
            {
                attacking = true;
                StartCoroutine(TimedAttack());
            }
        }
        else if (towerType == TowerType.Electric)
        {
            if (!attacking)
            {
                attacking = true;
                StartCoroutine(TimedAttack());
            }
        }
        else
        {
            targetEnemy = null;
        }
    }

    public void Attack()
    {
        // Deal damage if there's an enemy
        if (targetEnemy != null)
        {
            // Spawn a laser that gets sent towards the target
            targetRotation = targetEnemy.transform.position - transform.position;
            Instantiate(laserPrefab, laserSpawn.transform.position, Quaternion.LookRotation(targetRotation, Vector3.up));

            // Deal area of effect damage
            if (towerType == TowerType.Tank)
            {
                Array.Clear(aoeColliders, 0, aoeColliders.Length);
                int numColliders = Physics.OverlapSphereNonAlloc(targetEnemy.transform.position, aoeRadius, aoeColliders);
                for (int i = 0; i < numColliders; i++)
                {
                    if (aoeColliders[i].tag == "follow")
                    {
                        aoeColliders[i].gameObject.GetComponent<EnemyBehavior>().Damage(aoeDamage);
                    }
                }
            }

            // Damage target enemy
            targetEnemy.GetComponent<EnemyBehavior>().Damage(damage);
        }

        // Electric tower functionality
        if (towerType == TowerType.Electric)
        {
            shockParticle.Play();
            foreach (Collider enemy in hitColliders)
            {
                if (enemy != null && enemy.tag == "follow")
                {
                    enemy.gameObject.GetComponent<EnemyBehavior>().Shock(shockSpeed, shockTime);
                    enemy.gameObject.GetComponent<EnemyBehavior>().Damage(damage);
                }
            }
        }
    }

    public GameObject getTargetEnemy()
    {
        return targetEnemy;
    }

    private IEnumerator TimedAttack()
    {
        while (targetEnemy != null || towerType == TowerType.Electric)
        {
            Attack();
            WaitForSeconds wait = new WaitForSeconds(fireDelay);
            yield return wait;
        }
        attacking = false;
    }

}
