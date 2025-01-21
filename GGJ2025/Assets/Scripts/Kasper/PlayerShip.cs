using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    Rigidbody shipRB;

    //Inputs
    float verticalMove;
    float horizontalMove;
    float mouseInputX;
    float mouseInputY;
    float rollInput;

    //Speed Mulitipliers
    [SerializeField]
    float speedMult = 1;
    [SerializeField]
    float SpeedMultAngle = 0.5f;
    [SerializeField]
    float speedRollMultAngle = 0.05f;
    [SerializeField]
    float fuelConsumptionAmount = 5f;
    [SerializeField]
    float fuelConsumptionRate = 1.0f;


    float fuelRateTimer = 1.0f;

    [SerializeField]
    FuelSystem fuelSystem;

    [SerializeField]
    bool rotationUsesFuel;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        shipRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        verticalMove = Input.GetAxis("Vertical");
        horizontalMove = Input.GetAxis("Horizontal");
        rollInput = Input.GetAxis("Roll");

        mouseInputX = Input.GetAxis("Mouse X");
        mouseInputY = Input.GetAxis("Mouse Y");
    }

    //this function should only have physics/rigidbody code in here, avoid putting inputs here
    void FixedUpdate()
    {
        if(fuelSystem.currentFuel > 0)
        {
            shipRB.AddForce(shipRB.transform.TransformDirection(Vector3.forward) * verticalMove * speedMult, ForceMode.VelocityChange);
            shipRB.AddForce(shipRB.transform.TransformDirection(Vector3.right) * horizontalMove * speedMult, ForceMode.VelocityChange);

            shipRB.AddTorque(shipRB.transform.right * SpeedMultAngle * mouseInputY * -1, ForceMode.VelocityChange);
            shipRB.AddTorque(shipRB.transform.up * SpeedMultAngle * mouseInputX, ForceMode.VelocityChange);


            shipRB.AddTorque(shipRB.transform.forward * speedRollMultAngle * rollInput, ForceMode.VelocityChange);

            if (fuelRateTimer >= fuelConsumptionRate)
            {
                if (ShouldUseFuel())
                {
                    fuelSystem.UseFuel(fuelConsumptionAmount);
                    fuelRateTimer = 0;
                }

            }

            else
            {
                fuelRateTimer += Time.fixedDeltaTime;
            }
        }


    }

    public bool ShouldUseFuel()
    {
        bool isRotating = false;
        bool isMoving = false;

        if (HasAxisInput(rollInput) || HasAxisInput(mouseInputX) || HasAxisInput(mouseInputY))
        {
            isRotating = true;
        }
        
        if(HasAxisInput(verticalMove)  || HasAxisInput(horizontalMove))
        {
            isMoving = true;
        }

        if(rotationUsesFuel)
        {
            return isMoving || isRotating;
        }

        return isMoving;
    }

    public bool HasAxisInput(float value)
    {
        return (value > 0.1 || value < -0.1);
        
    }

}
