using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Phase { Placing, Pathing, Playing };

public class GameManager : MonoBehaviour
{

    public Text debug;

    public Phase gamePhase = Phase.Placing;

    public List<EnemySpawner> spawners;

    public int spawnerIndex = 0;

    public static GameManager instance;

    public static GameManager Instance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void addSpawner(EnemySpawner e)
    {
        spawners.Add(e);
        debug.text = "Added spawner!";
        
    }

    public Phase getGamePhase()
    {
        return gamePhase;
    }

    public void nextPhase()
    {
        switch(gamePhase)
        {
            case Phase.Placing:
                gamePhase = Phase.Pathing;
                break;
            case Phase.Pathing:
                gamePhase = Phase.Playing;
                break;
            default:
                gamePhase = Phase.Placing;
                break;
        }
    }

    public void nextSpawner()
    {
        spawnerIndex++;
    }
}
