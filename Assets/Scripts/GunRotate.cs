using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this script on the "gun" section of a turret.
/// The object associated with this script will rotate to look at the closest object within its radius
/// Ensure that objects you wish to be tracked are tagged "follow"
/// </summary>
public class GunRotate : MonoBehaviour
{
    public float radius = 10f;
    public float rotationSpeed = 2f;
    public ParticleSystem radiusParticle;

    private void Awake()
    {
        gameObject.GetComponent<LineRenderer>().SetPosition(0, new Vector3(transform.position.x, transform.position.y + .05f, transform.position.z));
        gameObject.GetComponent<LineRenderer>().enabled = false;
        var main = radiusParticle.main;
        main.startSize = radius * 2;
    }

    void FixedUpdate()
    {
        //Code source: https://docs.unity3d.com/ScriptReference/Physics.OverlapSphereNonAlloc.html
        int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders);
        float closestDistance = radius;
        int closestIndex = -1;
        for (int i = 0; i < numColliders; i++)
        {
            float distance = (hitColliders[i].transform.position - transform.position).magnitude;
            if (distance < closestDistance && hitColliders[i].tag == "follow")
            {
                closestIndex = i;
            }
        }

        if (closestIndex > -1)
        {
            //Debug.Log(hitColliders[closestIndex].tag);
            //Code source: https://answers.unity.com/questions/36255/lookat-to-only-rotate-on-y-axis-how.html

            /*
            Vector3 targetPosition = new Vector3(hitColliders[closestIndex].transform.position.x, transform.position.y, hitColliders[closestIndex].transform.position.z);
            this.transform.LookAt(targetPosition);
            */
            //The above way works just as well, but it doesn't give the cool lerp effect
            //Debug.Log("Targeting...");
            var lookPos = hitColliders[closestIndex].transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            gameObject.GetComponent<LineRenderer>().enabled = true;
            gameObject.GetComponent<LineRenderer>().SetPosition(1, hitColliders[closestIndex].transform.position);
            hitColliders[closestIndex].gameObject.GetComponent<EnemyBehavior>().Damage();
        }
        else
        {
            gameObject.GetComponent<LineRenderer>().enabled = false;
        }

    }
}
