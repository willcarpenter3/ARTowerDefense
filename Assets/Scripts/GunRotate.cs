﻿using System.Collections;
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
            if (distance < closestDistance)
            {
                closestIndex = i;
            }
        }

        if (closestIndex > -1)
        {
            if (hitColliders[closestIndex].tag == "follow")
            {
                //Code source: https://answers.unity.com/questions/36255/lookat-to-only-rotate-on-y-axis-how.html

                /*
                Vector3 targetPosition = new Vector3(hitColliders[closestIndex].transform.position.x, transform.position.y, hitColliders[closestIndex].transform.position.z);
                this.transform.LookAt(targetPosition);
                */
                //The above way works just as well, but it doesn't give the cool lerp effect

                var lookPos = hitColliders[closestIndex].transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            }
        }

    }
}