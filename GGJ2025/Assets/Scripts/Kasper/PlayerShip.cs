using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerShip : MonoBehaviour
{
    Rigidbody shipRB;

    // Inputs
    float verticalMove;
    float horizontalMove;
    float mouseInputX;
    float mouseInputY;
    float rollInput;

    // Speed Multipliers
    [SerializeField] float speedMult = 1;
    [SerializeField] float SpeedMultAngle = 0.5f;
    [SerializeField] float speedRollMultAngle = 0.05f;
    [SerializeField] float fuelConsumptionAmount = 5f;
    [SerializeField] float fuelConsumptionRate = 1.0f;

    // Fuel Related References
    float fuelRateTimer = 1.0f;
    [SerializeField] FuelSystem fuelSystem;
    [SerializeField] bool rotationUsesFuel;

    // Post-processing (Chromatic Aberration)
    public Volume postProcessingVolume;
    public ChromaticAberration globalVolume;

    public float shipVelocity;
    public float velocityMax = 1;

    [SerializeField] GameObject gameOverPanel;
    bool isReady;
    [SerializeField] private Animator animator;

    [SerializeField] private GroundDetector groundDetector; // Reference to ground detection
    [SerializeField] private float groundClearance = 2f; // Distance above the ground

    private void Awake()
    {
        shipRB = GetComponent<Rigidbody>();

        shipRB.linearVelocity = Vector3.zero;
        shipRB.angularVelocity = Vector3.zero;

        // Stabilization Fixes
        shipRB.angularDamping = 5f;  // Increases damping of unwanted rotation
        shipRB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // Improves collision physics
        shipRB.mass = 10f; // Makes the ship more stable
        shipRB.inertiaTensor = new Vector3(1, 1, 1) * 5f; // Adjust rotational inertia
    }

    private async void Start()
    {
        await Task.Delay(1000);
        isReady = true;
        //Cursor.lockState = CursorLockMode.Locked;

        if (postProcessingVolume.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            globalVolume = chromaticAberration;
        }
        else
        {
            Debug.Log("Missing post-processing effect.");
        }
    }

    void Update()
    {
        if (!isReady) return;

        float linearSpeed = shipRB.linearVelocity.magnitude;
        float angularSpeed = shipRB.angularVelocity.magnitude;

        shipVelocity = (1f * linearSpeed) + (0.5f * angularSpeed);

        float aberrationPercent = GetVelocityPercent();
        float aberrationIntensity = Mathf.Lerp(0, aberrationPercent, 1);
        globalVolume.intensity.value = aberrationIntensity;

        verticalMove = -1 * Input.GetAxis("Vertical");
        horizontalMove = -1 * Input.GetAxis("Horizontal");
        rollInput = -1 * Input.GetAxis("Roll");

        mouseInputX = Input.GetAxis("Mouse X");
        mouseInputY = -1 * Input.GetAxis("Mouse Y");

        AnimateShip();
    }

    void FixedUpdate()
    {
        if (!isReady) return;

        bool isGrounded = groundDetector.IsGrounded;

        if (fuelSystem.currentFuel > 0)
        {
            shipRB.AddForce(shipRB.transform.TransformDirection(Vector3.forward) * verticalMove * speedMult, ForceMode.VelocityChange);
            shipRB.AddForce(shipRB.transform.TransformDirection(Vector3.right) * horizontalMove * speedMult, ForceMode.VelocityChange);

            if (!isGrounded)
            {
                shipRB.AddTorque(shipRB.transform.right * SpeedMultAngle * mouseInputY * -1, ForceMode.VelocityChange);
                shipRB.AddTorque(shipRB.transform.up * SpeedMultAngle * mouseInputX, ForceMode.VelocityChange);
                shipRB.AddTorque(shipRB.transform.forward * speedRollMultAngle * rollInput, ForceMode.VelocityChange);
            }
            else
            {
                LevelShip();
            }

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

            // Stability Fixes
            shipRB.angularVelocity = Vector3.ClampMagnitude(shipRB.angularVelocity, 2f); // Limit rotational speed
            shipRB.angularVelocity *= 0.98f; // Smoothly reduce rotation over time

        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            gameOverPanel.SetActive(true);
        }
    }

    // Levels the ship with the ground
    void LevelShip()
    {
        Vector3 groundNormal = groundDetector.GroundNormal;
        Vector3 groundPoint = groundDetector.GroundPoint;

        if (groundPoint != Vector3.zero)
        {
            // Move ship to hover above the ground
            Vector3 targetPosition = groundPoint + (groundNormal * groundClearance);
            shipRB.MovePosition(Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * 5f));

            // Rotate the ship to align with the ground normal
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, groundNormal), groundNormal);
            shipRB.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f));
        }
    }

    public bool ShouldUseFuel()
    {
        bool isRotating = HasAxisInput(rollInput) || HasAxisInput(mouseInputX) || HasAxisInput(mouseInputY);
        bool isMoving = HasAxisInput(verticalMove) || HasAxisInput(horizontalMove);

        return rotationUsesFuel ? (isMoving || isRotating) : isMoving;
    }

    public bool HasAxisInput(float value)
    {
        return (value > 0.1f || value < -0.1f);
    }

    float GetVelocityPercent()
    {
        return Mathf.InverseLerp(0, velocityMax, shipVelocity);
    }

    public void AnimateShip()
    {
        animator.SetBool("isMoving", ShouldUseFuel());
    }

    // Reduce impact force on collision
    private void OnCollisionEnter(Collision collision)
    {
        shipRB.linearVelocity *= 0.5f;  // Reduce velocity on impact
        shipRB.angularVelocity *= 0.5f; // Reduce spin on impact
    }
}
