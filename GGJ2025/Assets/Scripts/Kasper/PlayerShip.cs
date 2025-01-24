using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
   
    //fuel related references
    float fuelRateTimer = 1.0f;
    [SerializeField]
    FuelSystem fuelSystem;
    [SerializeField]
    bool rotationUsesFuel;

    //chromaticaboration
    public Volume postProcessingVolume;
    public ChromaticAberration globalVolume;

    public float shipVelocity;
    public float velocityMax = 1;

    [SerializeField]
    GameObject gameOverPanel;
    bool isReady;
    [SerializeField]
    private Animator animator;


    private void Awake()
    {
        shipRB = GetComponent<Rigidbody>();

        shipRB.linearVelocity = Vector3.zero;
        shipRB.angularVelocity = Vector3.zero;
    }

    private async void Start()
    {
        await Task.Delay(1000);
        isReady = true;
        Cursor.lockState = CursorLockMode.Locked;

       if( postProcessingVolume.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            globalVolume = chromaticAberration;
        }
        else
        {
            Debug.Log("missing");
        }

    }

    void Update()
    {
       if(!isReady) return;

        float linearSpeed = shipRB.linearVelocity.magnitude;
        float angularSpeed = shipRB.angularVelocity.magnitude;

        shipVelocity = (1f * linearSpeed) + (0.5f * angularSpeed);

        float abberationpercent = GetVelocityPercent();
        float abberationIntensity = Mathf.Lerp(0, abberationpercent, 1);
        globalVolume.intensity.value = abberationIntensity;

        verticalMove = Input.GetAxis("Vertical");
        horizontalMove = Input.GetAxis("Horizontal");
        rollInput = Input.GetAxis("Roll");

        mouseInputX = Input.GetAxis("Mouse X");
        mouseInputY = Input.GetAxis("Mouse Y");

        AnimateShip();
    }

    //this function should only have physics/rigidbody code in here, avoid putting inputs here
    void FixedUpdate()
    {
        if(!isReady) return;

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
        else
        {
            Cursor.lockState = CursorLockMode.None;
            gameOverPanel.SetActive(true);
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

    float GetVelocityPercent()
    {
        return Mathf.InverseLerp(0, velocityMax , shipVelocity);
    }

    public void AnimateShip()
    {
        if (ShouldUseFuel())
        {
            animator.SetBool("isMoving", true);

        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

}
