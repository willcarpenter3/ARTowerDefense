using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Current Health of Object
    private float currentHealth;

    // Max Health of Object
    private float maxHealth;

    // Camera Object
    private Camera mainCamera;

    // Decides which health var to use
    public bool enemy;

    // Script Object to receive health var
    public EnemyBehavior enemyBehavior;
    public ObjectiveBehavior objectiveBehavior;

    // Bar Image
    public GameObject healthBar;

    // Set Current Health and Max Health
    void Start()
    {
        mainCamera = Camera.main;
        if (enemy)
        {
            currentHealth = enemyBehavior.health;
            maxHealth = enemyBehavior.health;
        }
        else
        {
            currentHealth = objectiveBehavior.health;
            maxHealth = objectiveBehavior.health;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // Set Health Bar to Enemy Health 
        if (enemy)
        {
            currentHealth = enemyBehavior.health;
        }
        // Set Health Bar to Objective Health 
        else
        {
            currentHealth = objectiveBehavior.health;
        }
        CameraUpdate();
        float scaleHealth = currentHealth / maxHealth;
        SetHealthBar(scaleHealth);
    }

    // Updates Health Bar to face Camera Object
    void CameraUpdate()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.back,
            mainCamera.transform.rotation * Vector3.up);
    }

    // Sets Health Bar Scale to Current Health
    void SetHealthBar(float myHealth)
    {
        healthBar.transform.localScale = new Vector3(myHealth, healthBar.transform.localScale.y, 
            healthBar.transform.localScale.z);
    }
}
