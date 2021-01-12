using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    /// <summary>
    /// Enum to define different enemy types
    /// </summary>
    public enum EnemyType { B1, B2, Droideka }

    [Header("Enemy Information")]
    
    public EnemyType enemyType; //Type of this enemy

    public int health = 100; //Health of this enemy unit

    private bool dead = false; //used to prevent ragdolling several times

    public float speed = 2f; //Speed at which this enemy moves

    private float currentSpeed; //used to stop the enemy during shock tower effect

    [Header("Droideka Shield info")]
    public Material transparentMat; //Completely transparent material - used for "removing" droideka shield

    public SkinnedMeshRenderer droidekaShield; //Renderer that renders the droideka shield

    [Header("Waypoint Information")]
    public List<Collider> waypoints; //List of all waypoints that this enemy should visit - assigned from spawner

    public int currentIndex = 0; //Which waypoint the enemy is currently navigating towards

    [Header("Misc Inputs")]

    public GameObject shockParticle; //Particle system with the shock/explosion effect for death

    private GameObject shockReference; //Keep a local copy of whatever shock particle is active

    //Other fields

    private bool inObjectiveRange = false; //used to stop the enemy when it has reached its objective

    private ObjectiveBehavior objective; //reference to the objective in the scene

    private Collider mainCollider;

    private Collider[] allColliders;

    //private Rigidbody mainRigidBody;

    private Rigidbody[] allRigidBodies;

    private void Start()
    {
        mainCollider = GetComponent<Collider>();
        //mainRigidBody = GetComponent<Rigidbody>();
        
        allColliders = gameObject.transform.GetChild(0).GetComponentsInChildren<Collider>(true);
        allRigidBodies = gameObject.transform.GetChild(0).GetComponentsInChildren<Rigidbody>(true);

        currentSpeed = speed;

        if (waypoints.Count > 0 && waypoints[currentIndex] != null)
        {
            Vector3 targetPosition = new Vector3(waypoints[currentIndex].transform.position.x, transform.position.y, waypoints[currentIndex].transform.position.z);
            transform.LookAt(targetPosition);
        }

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (health <= 0 && !dead)
        {
            mainCollider.isTrigger = false;
            
            gameObject.tag = "corpse";
            foreach(Collider c in allColliders)
            {
                c.gameObject.tag = "corpse";
            }
            dead = true;
            // Ragdoll Function
            Invoke("DoRagdoll", 0.25f);
            // Explosion Particle Effect
            //Destroy(gameObject, 2f); // Change to 10 secs  
            
        }

        if (health <= 25 && enemyType == EnemyType.Droideka)
        {
            Debug.Log("destroying shield");
            Material[] mats = droidekaShield.materials;
            mats[3] = transparentMat;
            droidekaShield.materials = mats;
        }

        
        if (!inObjectiveRange)
        {
            if (waypoints.Count > 0 && waypoints[currentIndex] != null)
            {
                Vector3 direction = Vector3.Normalize(waypoints[currentIndex].transform.position - transform.position);
                transform.position += direction * currentSpeed;
            }
            //else
            //{
            //    Debug.LogError("Ay bruh there ain't no goddamn waypoint to go to");
            //}
        } else
        {
            objective.Damage();
        }
        
    }

    private void DoRagdoll()
    {
        Debug.Log("ragdolling");
        this.enabled = false;
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().enabled = false;
        }
        foreach (var col in allColliders)
        {
            col.enabled = true;
        }
        foreach (var rb in allRigidBodies)
        {
            rb.isKinematic = false;
        }
        if (GameManager.Instance() != null)
        {
            GameManager.Instance().Invoke("checkWin", 0.1f);
        }

        shockReference = Instantiate(shockParticle, mainCollider.transform.position, transform.rotation);
        shockReference.transform.parent = gameObject.transform;
        if (enemyType == EnemyType.Droideka)
        {
            shockReference.transform.Translate(0, 0.05f, 0);
        }
        shockReference.GetComponent<ParticleSystem>().Play();
        Destroy(shockReference, .5f);

        
        if (enemyType == EnemyType.Droideka)
        {
            Destroy(gameObject, .5f);
        }
        else
        {
            Destroy(gameObject, 1.5f);
        }
        
        //Destroy(gameObject, .5f);
        

    }

    public void Damage()
    {
        health--;
    }

    public void Damage(int hit)
    {
        health -= hit;
    }

    public void EnterObjectiveRange(ObjectiveBehavior newObjective)
    {
        Debug.Log("entered objective range");

        inObjectiveRange = true;
        objective = newObjective;


        Vector3 targetPosition = new Vector3(objective.transform.position.x, objective.transform.position.y, objective.transform.position.z);
        transform.LookAt(targetPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Colliding " + other.gameObject.name);
        if (waypoints.Count > 0)
        {
            if (other == waypoints[currentIndex])
            {
                if (currentIndex < waypoints.Count - 1)
                {
                    currentIndex++;
                    Vector3 targetPosition = new Vector3(waypoints[currentIndex].transform.position.x, transform.position.y, waypoints[currentIndex].transform.position.z);
                    transform.LookAt(targetPosition);
                }
                
            }
        }
        if (other.gameObject.GetComponent<Bolt>() != null)
        {
            Destroy(other.gameObject);
        }
    }

    public void Shock(float newSpeed, float shockTime)
    {
        currentSpeed = newSpeed;
        shockReference = Instantiate(shockParticle, mainCollider.transform.position, transform.rotation);
        shockReference.transform.parent = gameObject.transform;
        shockReference.transform.Translate(0, 0.05f, 0);
        shockReference.GetComponent<ParticleSystem>().Play();
        StartCoroutine(Unshock(shockTime));
    }

    IEnumerator Unshock(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(shockReference);
        currentSpeed = speed;
    }
}
