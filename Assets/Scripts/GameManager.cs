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

    public GameObject winPanel;

    public GameObject losePanel;

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

        //debug.text = "Num enemies: " + FindObjectsOfType<EnemyBehavior>().Length;
    }

    public void gameLoss()
    {
        losePanel.SetActive(true);
    }

    public void gameWin()
    {
        winPanel.SetActive(true);
    }

    public void checkWin()
    {
        foreach (EnemySpawner spawn in spawners)
        {
            if (spawn.numEnemies > 0)
            {
                return;
            }
        }

        if (FindObjectsOfType<EnemyBehavior>().Length > 0)
        {
            return;
        }

        gameWin();
    }

    public void addSpawner(EnemySpawner e)
    {
        spawners.Add(e);
        //debug.text = "Added spawner!";
        
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
