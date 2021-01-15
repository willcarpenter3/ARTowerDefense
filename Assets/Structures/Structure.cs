using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Structure", menuName = "Structure")]
public class Structure : ScriptableObject
{
    public string structureName;
    public GameObject prefab;
    public Sprite image;
    public int cost;
}
