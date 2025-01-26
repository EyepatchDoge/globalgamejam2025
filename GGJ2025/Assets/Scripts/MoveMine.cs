using UnityEngine;

public class MoveMine : MonoBehaviour
{
    public float amplitude = 1f; // Height of the wave
    public float frequency = 1f; // Speed of the wave
    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
