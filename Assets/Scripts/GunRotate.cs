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
    public TowerAttack towerAttack;
    public float rotationSpeed = 2f;

    private void Awake()
    {

    }

    void FixedUpdate()
    {
        if (towerAttack.getTargetEnemy() != null)
        {
            //Debug.Log(hitColliders[closestIndex].tag);
            //Code source: https://answers.unity.com/questions/36255/lookat-to-only-rotate-on-y-axis-how.html

            /*
            Vector3 targetPosition = new Vector3(hitColliders[closestIndex].transform.position.x, transform.position.y, hitColliders[closestIndex].transform.position.z);
            this.transform.LookAt(targetPosition);
            */
            //The above way works just as well, but it doesn't give the cool lerp effect
            //Debug.Log("Targeting...");
            var lookPos = towerAttack.getTargetEnemy().transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }

    }
}
