using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SW_Char : MonoBehaviour
{
    public float health;

    public void DecreaseHealth(float damage)
    {
        if (health > damage) { health -= damage; }

        if (health <= damage) { health = 0; Kill(); }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
