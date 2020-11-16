using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public int health = 100;

    public float speed = 2f;

    private bool inObjectiveRange = false;

    private ObjectiveBehavior objective;

    public List<Collider> waypoints;

    public int currentIndex = 0;

    private void Start()
    {
        Vector3 targetPosition = new Vector3(waypoints[currentIndex].transform.position.x, transform.position.y, waypoints[currentIndex].transform.position.z);
        transform.LookAt(targetPosition);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        
        if (!inObjectiveRange)
        {
            if (waypoints[currentIndex] != null && waypoints.Count > 0)
            {
                Vector3 direction = Vector3.Normalize(waypoints[currentIndex].transform.position - transform.position);
                transform.position += direction * speed;
            }
            else
            {
                Debug.LogError("Ay bruh there ain't no goddamn waypoint to go to");
            }
        } else
        {
            objective.Damage();
        }
        
    }

    public void Damage()
    {
        health--;
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
        Debug.Log("Colliding");
        if (other == waypoints[currentIndex])
        {
            currentIndex++;
            Vector3 targetPosition = new Vector3(waypoints[currentIndex].transform.position.x, transform.position.y, waypoints[currentIndex].transform.position.z);
            transform.LookAt(targetPosition);
        }
    }
}
