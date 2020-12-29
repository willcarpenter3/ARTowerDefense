//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using static SW_Interfaces;

//public class ShipPlayer : MonoBehaviour, IDamageable
//{
//    [Header("Speed Details")]
//    [Tooltip("Current speed of ship")]
//    public float speed = 5;
//    [Tooltip("Minimum speed of ship")] [Min(0)]
//    public float minSpeed = 1;
//    [Tooltip("Maximum speed of ship")]
//    public float maxSpeed = 15;
//    public float speedSpinBoundary = 0.5f;

//    [Header("Sensitivity Details")]
//    public float speedSensitivity = 1;
//    public float spinSensitivity = 1;
//    public float turnSensitivity = 1;

//    [Header("Input Details")]
//    public InputAction speedInput;
//    public InputAction spinInput;
//    public InputAction turnInput;
//    private float changeInSpeed;
//    private float changeInSpin;
//    private Vector2 changeInTurn;


//    private void Awake()
//    {
//        speedInput.performed += ctx => changeInSpeed = ctx.ReadValue<Vector2>().y;
//        speedInput.canceled += ctx => changeInSpeed = 0;
//        speedInput.Enable();

//        spinInput.performed += ctx => changeInSpin = ctx.ReadValue<Vector2>().x;   // I'll try spinning,
//        spinInput.canceled += ctx => changeInSpin = 0;                             // that's a good trick!
//        spinInput.Enable();

//        turnInput.performed += ctx => changeInTurn = ctx.ReadValue<Vector2>();
//        turnInput.canceled += ctx => changeInTurn = Vector2.zero;
//        turnInput.Enable();

//    }

//    void ChangeSpeed(float speedChange)
//    {
//        speedChange /= 3;
//        speedChange *= speedSensitivity;

//        if (speed + speedChange >= maxSpeed) { speed = maxSpeed; }
//        else if (speed + speedChange <= minSpeed) { speed = minSpeed; }
//        else { speed += speedChange; }
//    }

//    void ChangeSpin(float spinChange)
//    {
//        if (changeInSpeed < speedSpinBoundary && changeInSpeed > -speedSpinBoundary)
//        {
//            spinChange *= spinSensitivity;
//            gameObject.transform.Rotate(new Vector3(0, 0, -spinChange));
//        }
//    }

//    void ChangeTurn(Vector2 turnChange)
//    {
//        turnChange *= turnSensitivity;
//        gameObject.transform.Rotate(new Vector3(-turnChange.y, turnChange.x, 0));
//    }

//    private void FixedUpdate()
//    {
//        ChangeSpeed(changeInSpeed);
//        ChangeSpin(changeInSpin);
//        ChangeTurn(changeInTurn);
//        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * speed);

//        // Praise be to Alberto Carrillo for helping with physics
//        LateralFriction();
//        VerticalFriction();
//    }

//    private void LateralFriction()
//    {
//        Rigidbody m_body = gameObject.GetComponent<Rigidbody>();

//        // faked lateral friction to limit sliding behavior
//        float lateralVelocty = Vector3.Dot(m_body.velocity, transform.right);

//        // calculate lateral friction force based on lateral velocity squared and the correction factors.
//        Vector3 lateralFrictionForce = -transform.right * Mathf.Sign(lateralVelocty) * lateralVelocty * lateralVelocty * 20f * Time.deltaTime * m_body.mass;

//        m_body.AddForce(lateralFrictionForce);
//        Debug.DrawRay(transform.position, lateralFrictionForce / 1000f);
//    }

//    private void VerticalFriction()
//    {
//        Rigidbody m_body = gameObject.GetComponent<Rigidbody>();

//        // faked vertical friction to limit sliding behavior
//        float verticalVelocity = Vector3.Dot(m_body.velocity, transform.up);

//        // calculate vertical friction force based on vertical velocity squared and the correction factors.
//        Vector3 verticalFrictionForce = -transform.right * Mathf.Sign(verticalVelocity) * verticalVelocity * verticalVelocity * 20f * Time.deltaTime * m_body.mass;

//        m_body.AddForce(verticalFrictionForce);
//        Debug.DrawRay(transform.position, verticalFrictionForce / 1000f);
//    }







//    public float health;

//    public void Damage(float damage)
//    {
//        if (health > damage) { health -= damage; }

//        if (health <= damage) { health = 0; Kill(); }
//    }

//    public void Kill()
//    {
//        Destroy(gameObject);
//    }
//}
