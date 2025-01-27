using UnityEngine;

public class MantaRayMover : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float yRandomness = 0.5f;
    [SerializeField] private float yOscillationSpeed = 1f;

    private float journeyLength;
    private float startTime;
    private bool movingToB = true;
    private float randomYOffset;
    private Quaternion originalRotation;

    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(pointA.position, pointB.position);
        randomYOffset = Random.Range(-yRandomness, yRandomness);
        originalRotation = transform.rotation; // Store original rotation
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;

        // Interpolates between A and B
        transform.position = Vector3.Lerp(movingToB ? pointA.position : pointB.position,
                                          movingToB ? pointB.position : pointA.position,
                                          fractionOfJourney);

        // Apply random Y oscillation
        float yOffset = Mathf.Sin(Time.time * yOscillationSpeed) * randomYOffset;
        transform.position += new Vector3(0, yOffset, 0);

        // Check if the object reached its target
        if (fractionOfJourney >= 1f)
        {
            movingToB = !movingToB;  // Toggle direction
            startTime = Time.time;   // Reset timing
            randomYOffset = Random.Range(-yRandomness, yRandomness); // New random Y oscillation

            // Flip the object by rotating 180 degrees on the Y-axis
            transform.rotation = originalRotation * Quaternion.Euler(0, movingToB ? 0 : 180, 0);
        }
    }
}