using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICredits : MonoBehaviour
{

    // FixedUpdate is called once per fixed time
    void FixedUpdate()
    {
        gameObject.GetComponent<TMP_Text>().text = GameManager.instance.numCredits.ToString();
    }
}
