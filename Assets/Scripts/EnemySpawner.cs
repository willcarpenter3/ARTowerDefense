using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public int numEnemies = 10;

    public float startDelay = 1f;

    public float delay = 2f;

    public List<Collider> waypoints;

    public GameObject enemyPrefab;

    bool started = false;

    bool empty = false;

    // Update is called once per frame
    void Update()
    {
        if (numEnemies <= 0 && !empty)
        {
            CancelInvoke();
            empty = true;
            GameManager.Instance().checkWin();
        }

        //Code inspired by this snarky jackass
        //src: https://answers.unity.com/questions/314815/delay-a-prefab-instantiate.html
        if (!started && GameManager.Instance().getGamePhase() == Phase.Playing)
        {
            InvokeRepeating("SpawnEnemy", startDelay, delay);
            //GameManager.Instance().debug.text = waypoints.Count.ToString();
            started = true;
        }


        //TODO get rid of this, it's for testing purposes only
        if (Input.GetKeyUp(KeyCode.Space) && (GameManager.Instance().getGamePhase() == Phase.Placing))
        {
            GameManager.Instance().addSpawner(this);
            GameManager.Instance().nextPhase();
            GameManager.Instance().nextPhase();
            GameManager.Instance().nextSpawner();
        }
    }

    void SpawnEnemy()
    {
        //src: https://docs.unity3d.com/2019.3/Documentation/Manual/InstantiatingPrefabs.html
        GameObject instance = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        instance.GetComponent<EnemyBehavior>().waypoints = waypoints;
        numEnemies--;
    }

    public void addWaypoint(Collider c)
    {
        waypoints.Add(c);
    }
}
