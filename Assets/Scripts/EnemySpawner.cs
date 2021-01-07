using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public int enemiesToSpawn = 10;

    private int numEnemies;

    public float startDelay = 1f;

    public float delay = 2f;

    public List<Collider> waypoints;

    public GameObject enemyPrefab;

    public bool started = false;

    public bool empty = false;

    private LineRenderer lineRenderer;

    private void Start()
    {
        numEnemies = enemiesToSpawn;
    }

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
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("bruh");
            GameManager.Instance().addSpawner(this);
            GameManager.Instance().nextPhase(Phase.Playing);
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
        //DrawLine(waypoints[waypoints.Count - 1].transform.position);
    }

    public void ResetSpawner()
    {
        started = false;
        empty = false;
        numEnemies = enemiesToSpawn;
    }

    /*
    private void DrawLine(Vector3 end)
    {
        GameObject line = new GameObject();
        line.transform.position = waypoints[waypoints.Count - 1].transform.position;
        line.AddComponent<LineRenderer>();
        lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(waypoints.Count - 2, line.transform.position);
        lineRenderer.SetPosition(waypoints.Count - 1, end);
        // Wide render
        // Low to the ground (plane)
        // Translucent
        // Color based on spawner
    }
    */
}
