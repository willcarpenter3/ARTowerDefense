using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public int health = 100;

    private bool inObjectiveRange = false;

    private ObjectiveBehavior objective;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        
        if (!inObjectiveRange)
        {
            //TEMP GET RID OF THIS
            transform.position += new Vector3(0, 0, 0.3f);
            //PUT ACTUAL LOGIC FOR FOLLOWING PATHS HERE
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
}
