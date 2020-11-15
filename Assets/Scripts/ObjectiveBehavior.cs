using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveBehavior : MonoBehaviour
{

    public int health = 1000;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Damage()
    {
        health--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "follow")
        {
            other.gameObject.GetComponent<EnemyBehavior>().EnterObjectiveRange(this);
        }
    }
}
