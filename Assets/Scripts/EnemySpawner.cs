using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    //Public Fields
    [Header("Spawner Settings")]
    public int enemiesToSpawn = 10;

    public int roundToStart = 1;

    public float startDelay = 1f;

    public float spawnDelay = 2f;

    [Header("Type Information")]

    public GameObject enemyPrefab;

    public EnemyBehavior.EnemyType enemyType;

    public Material lineMaterial;

    [Header("Status Information")]

    public bool started = false;

    public bool empty = false;

    public List<Collider> waypoints;

    //Private Fields

    private int enemiesToAdd = 2;

    public int numEnemies;

    public float widthMultiplier = 0.0125f;

    private LineRenderer lineRenderer;

    private GameObject line;    

    private void Start()
    {
        numEnemies = enemiesToSpawn;

        CreateLine();
        AddPointToLine(waypoints[1].transform.position, 1);
        AddPointToLine(waypoints[2].transform.position, 2);

    }
    // Update is called once per frame
    void Update()
    {
        if (numEnemies <= 0 && !empty)
        {
            CancelInvoke();
            empty = true;
            //GameManager.Instance().checkWin();
        }

        //Code inspired by this snarky jackass
        //src: https://answers.unity.com/questions/314815/delay-a-prefab-instantiate.html
        if (!started && GameManager.Instance().getGamePhase() == Phase.Playing && GameManager.Instance().getRoundNumber() >= roundToStart)
        {
            InvokeRepeating("SpawnEnemy", startDelay, spawnDelay);
            //GameManager.Instance().debug.text = waypoints.Count.ToString();
            started = true;
        }


        //TODO get rid of this, it's for testing purposes only
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("bruh");
            //GameManager.Instance().addSpawner(this);
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
        
        AddPointToLine(waypoints[waypoints.Count - 1].transform.position, waypoints.Count);
    }

    public void ResetSpawner()
    {
        if (GameManager.Instance().getRoundNumber() >= roundToStart)
        {
            started = false;
            empty = false;
            numEnemies = enemiesToSpawn + (enemiesToAdd * (GameManager.Instance().getRoundNumber() - roundToStart));
        }
    }

    public int getNumEnemies()
    {
        //Don't report enemies until it is their time to go
        if (GameManager.Instance().getRoundNumber() >= roundToStart)
        {
            return numEnemies;
        }
        else
        {
            return 0;
        }
    }

    
    // Create Line
    private void CreateLine()
    {
        line = new GameObject();
        line.name = "Line";
        line.transform.position = transform.position;
        line.AddComponent<LineRenderer>();
        lineRenderer = line.GetComponent<LineRenderer>();
        Vector3 spawnerPosition = line.transform.position;
        //spawnerPosition.y -= 0.0001f;
        lineRenderer.SetPosition(0, spawnerPosition);
        // Wide render
        lineRenderer.widthMultiplier = widthMultiplier;
        lineRenderer.material = lineMaterial;
        // Color based on spawner
        if (enemyType == EnemyBehavior.EnemyType.B1)
        {

            lineRenderer.startColor = new Color(244f / 255f, 129f / 255f, 0f / 255f, 1f);
            lineRenderer.endColor = new Color(244f / 255f, 129f / 255f, 0f / 255f, 1f);
        }
        else if (enemyType == EnemyBehavior.EnemyType.B2)
        {
            lineRenderer.startColor = new Color(70f / 255f, 80f / 255f, 87f / 255f, 1f);
            lineRenderer.endColor = new Color(70f / 255f, 80f / 255f, 87f / 255f, 1f);
        }
        else if (enemyType == EnemyBehavior.EnemyType.Droideka)
        {
            lineRenderer.startColor = new Color(89f / 255f, 183f / 255f, 255f / 255f, 1f);
            lineRenderer.endColor = new Color(89f / 255f, 183f / 255f, 255f / 255f, 1f);
        }
        lineRenderer.enabled = false;
    }


    // Add Point to Line
    private void AddPointToLine(Vector3 point, int index)
    {
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }
        // Adds to positions to vertices array
        if (lineRenderer.positionCount <= index)
        {
            lineRenderer.positionCount += 1;
        }
        lineRenderer.SetPosition(index, point);
    }

}
