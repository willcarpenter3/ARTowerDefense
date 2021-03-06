﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Phase { PlaneSelection, Placing, Pathing, Playing };

public class GameManager : MonoBehaviour
{

    public Text debug;

    public Phase gamePhase = Phase.Placing;

    public List<EnemySpawner> spawners;

    public int spawnerIndex = 0;

    public GameObject winPanel;

    public GameObject losePanel;

    public TMP_Text phaseDesc;

    public ObjectMenu structureMenu;

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
            case Phase.PlaneSelection:
                gamePhase = Phase.Placing;
                structureMenu.ChangeMenu(structureMenu.placingStructures);
                phaseDesc.text = "Place the objective, enemy spawners, and defense towers";
                break;
            case Phase.Placing:
                gamePhase = Phase.Pathing;
                structureMenu.ChangeMenu(structureMenu.pathingStructures);
                phaseDesc.text = "Place waypoints for the path of the designated spawner's enemies";
                break;
            case Phase.Pathing:
                gamePhase = Phase.Playing;
                structureMenu.ChangeMenu(structureMenu.playingStructures);
                phaseDesc.text = "Will the enemies get defeated before they destroy the objective?";
                break;
            default:
                gamePhase = Phase.PlaneSelection;
                phaseDesc.text = "Select the plane you want to play on";
                break;
        }
    }

    public void nextSpawner()
    {
        spawnerIndex++;
    }
}
