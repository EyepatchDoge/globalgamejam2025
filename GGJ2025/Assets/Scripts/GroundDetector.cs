using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded = false;
    private Vector3 groundNormal = Vector3.up;
    private Vector3 groundPoint = Vector3.zero;

    public bool IsGrounded => isGrounded;
    public Vector3 GroundNormal => groundNormal;
    public Vector3 GroundPoint => groundPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
            {
                groundNormal = hit.normal;
                groundPoint = hit.point;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
}
