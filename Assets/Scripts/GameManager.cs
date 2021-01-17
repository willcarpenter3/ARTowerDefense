using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Phase { PlaneSelection, ObjectivePlacing, SpawnerPlacing, Pathing, TowerPlacing, Playing };

public class GameManager : MonoBehaviour
{

    public Text debug;

    public Phase gamePhase = Phase.PlaneSelection;

    public List<EnemySpawner> spawners;

    public int spawnerIndex = 0;

    public GameObject winPanel;

    public GameObject losePanel;

    public TMP_Text phaseDesc;

    public ObjectMenu structureMenu;

    public GameObject selectedEffect;

    public GameObject confirmButton;

    public int numCredits;

    private int roundAllowanceMultiplier = 1;

    private GameObject currentSelectedEffect;

    //Round Logic
    public int roundsToWin = 15;

    private int roundNumber = 1;

    public int enemiesThisRound;

    //Singleton Game Manager Logic
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
        confirmButton.SetActive(false);
        losePanel.SetActive(true);
    }

    public void gameWin()
    {
        confirmButton.SetActive(false);
        winPanel.SetActive(true);
    }

    public void checkWin()
    {
        /*
        foreach (EnemySpawner spawn in spawners)
        {
            if (!spawn.empty && spawn.started)
            {
                return;
            }
        }

        if (FindObjectsOfType<EnemyBehavior>().Length > 0)
        {
            return;
        }*/

        enemiesThisRound--;

        Debug.Log("Yeeting Enemy");

        if (enemiesThisRound <= 0)
        {
            if (roundNumber == roundsToWin)
            {
                //Win the game and either quit or jump into endless mode
                gameWin();
            }
            else
            {
                //Advance to the next round
                roundNumber++;
                //Reset Spawners
                foreach (EnemySpawner spawn in spawners)
                {
                    spawn.ResetSpawner();
                }
                nextPhase();
                debug.text = "Round Number: " + roundNumber;
            }
        }
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
            case Phase.PlaneSelection:
                gamePhase = Phase.ObjectivePlacing;
                structureMenu.ChangeMenu(structureMenu.objectiveStructures);
                phaseDesc.text = "Place your base (Choose wisely, you can only place one!)";
                break;
            case Phase.ObjectivePlacing:
                gamePhase = Phase.SpawnerPlacing;
                structureMenu.ChangeMenu(structureMenu.spawnerStructures);
                phaseDesc.text = "Place spawners for the different enemy types";
                confirmButton.SetActive(true);
                confirmButton.GetComponentInChildren<Text>().text = "Begin Pathing";
                break;
            case Phase.SpawnerPlacing:
                gamePhase = Phase.Pathing;
                structureMenu.ChangeMenu(structureMenu.pathingStructures);
                phaseDesc.text = "Place waypoints for the path of the designated spawner's enemies";
                addSelectedEffect();
                confirmButton.GetComponentInChildren<Text>().text = "Next Path";
                break;
            case Phase.Pathing:
                addAllowanceFirstRound();
                gamePhase = Phase.TowerPlacing;
                structureMenu.ChangeMenu(structureMenu.towerStructures);
                phaseDesc.text = "Place towers to defend your base!";
                hideWaypoints();
                removeSelectedEffect();
                confirmButton.GetComponentInChildren<Text>().text = "Start Battle";
                break;
            case Phase.TowerPlacing:
                gamePhase = Phase.Playing;
                GetNumEnemies();
                structureMenu.ChangeMenu(structureMenu.playingStructures);
                phaseDesc.text = "Will the enemies get defeated before they destroy the objective?";
                confirmButton.SetActive(false);
                phaseDesc.gameObject.transform.parent.gameObject.SetActive(false);
                structureMenu.gameObject.SetActive(false);
                break;
            case Phase.Playing:
                addAllowanceRound();
                structureMenu.gameObject.SetActive(true);
                phaseDesc.gameObject.transform.parent.gameObject.SetActive(true);
                gamePhase = Phase.TowerPlacing;
                structureMenu.ChangeMenu(structureMenu.towerStructures);
                phaseDesc.text = "Place towers to defend your base!";
                //removeSelectedEffect();
                confirmButton.SetActive(true);
                confirmButton.GetComponentInChildren<Text>().text = "Next Round";
                break;
            default:
                gamePhase = Phase.PlaneSelection;
                phaseDesc.text = "Select the plane you want to play on";
                break;
        }
    }

    public void nextPhase(Phase phase)
    {
        gamePhase = phase - 1;
        nextPhase();
    }

    public void nextSpawner()
    {
        spawnerIndex++;
        removeSelectedEffect();
        addSelectedEffect();
    }

    public int getRoundNumber()
    {
        return roundNumber;
    }

    private void addSelectedEffect()
    {
        currentSelectedEffect = Instantiate(selectedEffect, spawners[spawnerIndex].gameObject.transform);
    }

    private void removeSelectedEffect()
    {
        Destroy(currentSelectedEffect);
        currentSelectedEffect = null;
    }

    private void GetNumEnemies()
    {
        enemiesThisRound = 0;
        foreach (EnemySpawner spawn in spawners)
        {
            Debug.Log("Adding " + spawn.getNumEnemies() + "from spawner");
            enemiesThisRound += spawn.getNumEnemies(); 
        }
    }

    private void hideWaypoints()
    {
        foreach (EnemySpawner spawner in spawners)
        {
            foreach (Collider waypoint in spawner.waypoints)
            {
                if (waypoint.gameObject.CompareTag("waypoint"))
                {
                    waypoint.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                }
            }
        }
    }

    public void addAllowanceEnemy(EnemyBehavior.EnemyType type)
    {
        if (type == EnemyBehavior.EnemyType.B1)
        {
            numCredits += 1;
        }
        else if (type == EnemyBehavior.EnemyType.B2)
        {
            numCredits += 2;
        }
        else if (type == EnemyBehavior.EnemyType.Droideka)
        {
            numCredits += 5;
        }

    }

    public void addAllowanceRound()
    {
        //numCredits += spawners.Count * 10;
        numCredits += (roundNumber - 1) * 10;
    }

    public void addAllowanceFirstRound()
    {
        numCredits += spawners.Count * 15;
    }

    public void spendCredits(int cost)
    {
        numCredits -= cost;
    }
}
