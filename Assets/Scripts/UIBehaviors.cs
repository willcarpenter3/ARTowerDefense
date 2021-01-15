using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIBehaviors : MonoBehaviour
{
    public void ActivateObject(GameObject other)
    {
        other.SetActive(true);
    }

    public void DeactivateObject(GameObject other)
    {
        other.SetActive(false);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
