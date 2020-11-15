using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ObjectMenu : MonoBehaviour
{
    public List<Structure> structures = new List<Structure>();
    public GameObject buttonPrefab;
    public GameObject contentWindow;

    private GameObject selectedStructure;

    public void Awake()
    {
        foreach(Structure obj in structures)
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
}
