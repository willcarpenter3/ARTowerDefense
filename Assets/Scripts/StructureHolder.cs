using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class StructureHolder : MonoBehaviour
{
    public Structure structure;
    public ObjectMenu menu;
    public Toggle toggle;
    public TMP_Text costTxt;
    public TMP_Text nameTxt;
    public GameObject creditSymbol;
    public GameObject inactiveBox;

    public void SetSelectedStructure()
    {
        if (gameObject.GetComponent<Toggle>().isOn)
        {
            menu.SetSelectedStructure(structure.prefab);
        }
    }

    private void FixedUpdate()
    {
        if (structure.cost > 0 && structure.cost > GameManager.Instance().numCredits)
        {
            toggle.enabled = false;
            inactiveBox.SetActive(true);
        }
        else if (toggle.isOn)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
    }

    private void Start()
    {
        costTxt.text = structure.cost.ToString();
        nameTxt.text = structure.structureName;

        if (structure.cost == 0)
        {
            costTxt.gameObject.SetActive(false);
            creditSymbol.SetActive(false);
        }
    }
}
