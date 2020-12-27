//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class Blaster : MonoBehaviour
//{
//    [Header("Bolt Details")]
//    [Tooltip("Prefab of the bolt that will be shot")]
//    public GameObject boltPrefab;
//    [Tooltip("Speed of the bolt in the air")] [Min(0)]
//    public float boltSpeed = 1;
//    [Tooltip("Damage of the bolt when it hits")] [Min(0)]
//    public float boltDamage = 10;

//    [Header("Reload Details")]
//    [Tooltip("Multiplied by the current heat value to get the reload time in seconds")] [Min(0)]
//    public float reloadMultiplier = 0.02f;
//    [Tooltip("Amount of heat available")] [Min(0)]
//    public float heatCapacity = 100;
//    [Tooltip("Amount of heat per bolt. Set to 0 for no reload needed")] [Min(0)]
//    public float heatPerBolt = 10;
//    private float currentHeat = 0;
//    private float reloadTimeStamp = 0;

//    [Header("Shoot Details")]
//    [Tooltip("Minimum seconds between each shot")] [Min(0)]
//    public float shootRate = 0.1f;
//    private float shootRateTimeStamp = 0;
//    private AudioSource blasterAudio;

//    [Header("Input Details")]
//    public InputAction shootInput;
//    public InputAction reloadInput;

//    void Start()
//    {
//        shootInput.performed += ctx => ShootBolt();
//        shootInput.Enable();

//        reloadInput.performed += ctx => Reload();
//        reloadInput.Enable();

//        blasterAudio = gameObject.GetComponent<AudioSource>();
//    }

//    void ShootBolt()
//    {
//        if (Time.time > shootRateTimeStamp && currentHeat < heatCapacity && Time.time > reloadTimeStamp)
//        {
//            blasterAudio.Play();
//            shootRateTimeStamp = Time.time + shootRate;
//            GameObject laser = Instantiate(boltPrefab, transform.position, transform.rotation);
//            laser.GetComponent<Bolt>().SetBlaster(gameObject);
//            laser.GetComponent<Bolt>().speed = boltSpeed;
//            laser.GetComponent<Bolt>().damage = boltDamage;
//            currentHeat += heatPerBolt;
//        }
//    }

//    void Reload()
//    {
//        if (currentHeat > 0 && Time.time > reloadTimeStamp)
//        {
//            Debug.Log("CURRENT TIME: " + Time.time);
//            reloadTimeStamp = Time.time + (currentHeat * reloadMultiplier);
//            Debug.Log("FINISH RELOADING AT: " + reloadTimeStamp);
//            currentHeat = 0;
//        }
//    }
//}
