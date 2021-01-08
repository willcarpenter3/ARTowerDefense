﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public int numEnemies = 10;

    public float startDelay = 1f;

    public float delay = 2f;

    public List<Collider> waypoints;

    public GameObject enemyPrefab;

    public bool started = false;

    public bool empty = false;

    private LineRenderer lineRenderer;

    private GameObject line;

    private void Start()
    {
        CreateLine();
        AddPointToLine(waypoints[0].transform.position, 1);
        AddPointToLine(waypoints[1].transform.position, 2);
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
        //lineRenderer
        // Low to the ground (plane)
        // Translucent
        // Color based on spawner
        //lineRenderer.material = new Material();
    }


    // Add Point to Line
    private void AddPointToLine(Vector3 point, int index)
    {
        lineRenderer = line.GetComponent<LineRenderer>();
        if (lineRenderer.positionCount <= index)
        {
            lineRenderer.positionCount += 1;
        }
        lineRenderer.SetPosition(index, point);
        // Wide render
        lineRenderer.widthMultiplier = 0.025f;
        // Low to the ground (plane)
        // Translucent
        // Color based on spawner
    }

    // End Line (Last waypoint to Objective)
    public void EndLine(Vector3 end)
    {
        lineRenderer.SetPosition(waypoints.Count, end);
        // Wide render
        // Low to the ground (plane)
        // Translucent
        // Color based on spawner
    }
}
