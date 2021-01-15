﻿using System.Collections;
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
        losePanel.SetActive(true);
    }

    public void gameWin()
    {
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
                GetNumEnemies();
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
                break;
            case Phase.SpawnerPlacing:
                gamePhase = Phase.Pathing;
                structureMenu.ChangeMenu(structureMenu.pathingStructures);
                phaseDesc.text = "Place waypoints for the path of the designated spawner's enemies";
                addSelectedEffect();
                break;
            case Phase.Pathing:
                gamePhase = Phase.TowerPlacing;
                structureMenu.ChangeMenu(structureMenu.towerStructures);
                phaseDesc.text = "Place towers to defend your base!";
                hideWaypoints();
                removeSelectedEffect();
                break;
            case Phase.TowerPlacing:
                gamePhase = Phase.Playing;
                structureMenu.ChangeMenu(structureMenu.playingStructures);
                phaseDesc.text = "Will the enemies get defeated before they destroy the objective?";
                break;
            case Phase.Playing:
                gamePhase = Phase.TowerPlacing;
                structureMenu.ChangeMenu(structureMenu.towerStructures);
                phaseDesc.text = "Place towers to defend your base!";
                removeSelectedEffect();
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
}
