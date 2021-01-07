using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ObjectMenu : MonoBehaviour
{
    public List<Structure> objectiveStructures = new List<Structure>();
    public List<Structure> spawnerStructures = new List<Structure>();
    public List<Structure> pathingStructures = new List<Structure>();
    public List<Structure> towerStructures = new List<Structure>();
    public List<Structure> playingStructures = new List<Structure>();
    public GameObject buttonPrefab;
    public GameObject contentWindow;

    private GameObject selectedStructure;

    public void Awake()
    {
        
    }

    public void ChangeMenu(List<Structure> structures)
    {
        foreach (Transform child in contentWindow.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Structure obj in structures)
        {
            GameObject button = Instantiate(buttonPrefab, contentWindow.transform);
            button.GetComponent<Image>().sprite = obj.image;
            button.GetComponent<Toggle>().group = contentWindow.GetComponent<ToggleGroup>();
            button.GetComponent<StructureHolder>().structure = obj;
            button.GetComponent<StructureHolder>().menu = this;
        }
    }

    public void SetSelectedStructure(GameObject structure)
    {
        selectedStructure = structure;
    }

    public GameObject GetSelectedStructure()
    {
        return selectedStructure;
    }
}
