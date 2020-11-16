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

    // Start is called before the first frame update
    void Start()
    {
        //Code inspired by this snarky jackass
        //src: https://answers.unity.com/questions/314815/delay-a-prefab-instantiate.html
        InvokeRepeating("SpawnEnemy", startDelay, delay);
    }

    // Update is called once per frame
    void Update()
    {
        if (numEnemies <= 0)
        {
            CancelInvoke();
        }
    }

    void SpawnEnemy()
    {
        //src: https://docs.unity3d.com/2019.3/Documentation/Manual/InstantiatingPrefabs.html
        GameObject instance = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        instance.GetComponent<EnemyBehavior>().waypoints = waypoints;
        numEnemies--;
    }
}
