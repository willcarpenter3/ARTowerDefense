using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StructureHolder : MonoBehaviour
{
    public Structure structure;
    public ObjectMenu menu;

    public void SetSelectedStructure()
    {
        if (gameObject.GetComponent<Toggle>().isOn)
        {
            menu.SetSelectedStructure(structure.prefab);
        }
    }
}
