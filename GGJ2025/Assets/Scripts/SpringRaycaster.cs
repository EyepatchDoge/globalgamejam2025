using System.Collections.Generic;
using UnityEngine;

public class SpringRaycaster : MonoBehaviour
{
    [Header("Raycast Settings")]
    public List<Transform> raycastPoints = new List<Transform>();
    public float raycastDistance = 10f;
    public LayerMask raycastLayerMask;

    [Header("Spring Settings")]
    public float desiredDistance = 5f;
    public float springStrength = 10f;
    public float springDamping = 5f;
    public float maxVelocity = 10f;

    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (raycastPoints.Count == 0) return;

        Vector3 totalSpringForce = Vector3.zero;
        int hitCount = 0;

        foreach (Transform point in raycastPoints)
        {
            if (point == null) continue;

            Ray ray = new Ray(point.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, raycastLayerMask))
            {
                Vector3 targetPosition = hit.point + Vector3.up * desiredDistance;
                Vector3 displacement = targetPosition - transform.position;
                float scaledSpringStrength = springStrength * (1f - (hit.distance / raycastDistance));

                totalSpringForce += displacement * scaledSpringStrength;
                hitCount++;

                Debug.DrawLine(point.position, hit.point, Color.red);
                Debug.DrawRay(hit.point, Vector3.up * 0.5f, Color.blue);
            }
            else
            {
                Debug.DrawLine(point.position, point.position + Vector3.down * raycastDistance, Color.gray);
            }
        }

        if (hitCount > 0)
        {
            totalSpringForce /= hitCount;

            Vector3 dampingForce = -velocity * springDamping;
            velocity += (totalSpringForce + dampingForce) * Time.fixedDeltaTime;

            // Limit velocity for stability
            if (velocity.magnitude > maxVelocity)
                velocity = velocity.normalized * maxVelocity;

            transform.position += velocity * Time.fixedDeltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        if (raycastPoints == null || raycastPoints.Count == 0) return;

        Gizmos.color = Color.green;

        foreach (Transform point in raycastPoints)
        {
            if (point == null) continue;
            Gizmos.DrawLine(point.position, point.position + Vector3.down * raycastDistance);
        }
    }
}
