using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class StructureHolder : MonoBehaviour
{
    public Structure structure;
    public ObjectMenu menu;
    public Toggle toggle;

    public void SetSelectedStructure()
    {
        if (gameObject.GetComponent<Toggle>().isOn)
        {
            menu.SetSelectedStructure(structure.prefab);
        }
    }

    private void FixedUpdate()
    {
        if (toggle.isOn)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
    }
}
