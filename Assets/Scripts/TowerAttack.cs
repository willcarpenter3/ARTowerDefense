using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerAttack : MonoBehaviour
{
    public TowerType towerType;

    [Header("Standard Settings")]
    public float radius = 10f;
    public float fireDelay = 1f;
    public int damage = 10;

    [Header("Visual Settings")]
    public ParticleSystem radiusParticle;
    public GameObject laserSpawn;
    public GameObject laserPrefab;

    [Header("Tank Settings")]
    public float aoeRadius = 10f;
    public int aoeDamage = 5;


    private GameObject targetEnemy;
    private bool attacking = false;

    // Max detection of 10 enemies at a time
    private readonly Collider[] hitColliders = new Collider[10];
    private Vector3 targetRotation;
    private readonly Collider[] aoeColliders = new Collider[10];



    public enum TowerType
    {
        Standard,
        Scout,
        Tank,
        Sniper,
        Electric
    }




    private void Awake()
    {
        var main = radiusParticle.main;
        main.startSize = radius * 2;
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
                }
            }
            else
            {
                if (distance > longestDistance && hitColliders[i].tag == "follow")
                {
                    targetIndex = i;
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
        else
        {
            targetEnemy = null;
            attacking = false;
        }
    }

    public void Attack()
    {
        // Spawn a laser that gets sent towards the target
        targetRotation = targetEnemy.transform.position - transform.position;
        Instantiate(laserPrefab, laserSpawn.transform.position, Quaternion.LookRotation(targetRotation, Vector3.up));

        // Deal damage
        if (targetEnemy != null)
        {
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

            targetEnemy.GetComponent<EnemyBehavior>().Damage(damage);
        }
    }

    public GameObject getTargetEnemy()
    {
        return targetEnemy;
    }

    private IEnumerator TimedAttack()
    {
        while (targetEnemy != null)
        {
            Attack();
            WaitForSeconds wait = new WaitForSeconds(fireDelay);
            yield return wait;
        }
    }

}
