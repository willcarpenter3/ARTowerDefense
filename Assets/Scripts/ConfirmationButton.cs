using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationButton : MonoBehaviour
{
    public void Confirm()
    {
        if (GameManager.Instance().getGamePhase() == Phase.Pathing)
        {

            //This is ugly as sin and it only works with one objective
            GameManager.Instance().spawners[GameManager.Instance().spawnerIndex].addWaypoint(FindObjectsOfType<ObjectiveBehavior>()[0].gameObject.GetComponent<Collider>());

            if (GameManager.Instance().spawnerIndex == GameManager.Instance().spawners.Count - 1)
            {
                GameManager.Instance().nextPhase();
            }
            else
            {
                GameManager.Instance().nextSpawner();
            }            
        }
        else
        {
            GameManager.Instance().nextPhase();
        }
    }
}
