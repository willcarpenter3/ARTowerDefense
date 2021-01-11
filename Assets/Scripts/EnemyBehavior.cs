using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public enum EnemyType { B1, B2, Droideka}

    public EnemyType enemyType;

    public int health = 100;

    public float speed = 2f;

    public Material transparentMat;

    public SkinnedMeshRenderer droidekaShield;

    private bool inObjectiveRange = false;

    private ObjectiveBehavior objective;

    public List<Collider> waypoints;

    public int currentIndex = 0;

    private float currentSpeed;

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
        if (health <= 0)
        {
            mainCollider.isTrigger = false;
            
            gameObject.tag = "corpse";
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

        Destroy(gameObject);

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
        StartCoroutine(Unshock(shockTime));
    }

    IEnumerator Unshock(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        currentSpeed = speed;
    }
}
