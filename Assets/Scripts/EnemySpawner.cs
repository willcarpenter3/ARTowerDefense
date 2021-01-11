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

    private GameObject line;

    public EnemyBehavior.EnemyType enemyType;

    public Material lineMaterial;

    private void Start()
    {
        numEnemies = enemiesToSpawn;

        CreateLine();
        //AddPointToLine(waypoints[0].transform.position, 1);
        //AddPointToLine(waypoints[1].transform.position, 2);
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
        
        AddPointToLine(waypoints[waypoints.Count - 1].transform.position, waypoints.Count);
    }

    public void ResetSpawner()
    {
        started = false;
        empty = false;
        numEnemies = enemiesToSpawn;
    }

    
    // Create Line
    private void CreateLine()
    {
        line = new GameObject();
        line.name = "Line";
        line.transform.position = transform.position;
        line.AddComponent<LineRenderer>();
        lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, line.transform.position);
        // Wide render
        lineRenderer.widthMultiplier = 0.025f;
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
    }


    // Add Point to Line
    private void AddPointToLine(Vector3 point, int index)
    {
        // Adds to positions to vertices array
        if (lineRenderer.positionCount <= index)
        {
            lineRenderer.positionCount += 1;
        }
        lineRenderer.SetPosition(index, point);
    }

}
