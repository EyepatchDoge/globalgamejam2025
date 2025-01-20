using UnityEngine;

public class EndingTriggerArea : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if ( rb != null && other.CompareTag("Player"))
        {
            Debug.Log("player made it to the finish");
        }

        else
        {
            Debug.Log("Rigidbody missing");
        }
    }

}
