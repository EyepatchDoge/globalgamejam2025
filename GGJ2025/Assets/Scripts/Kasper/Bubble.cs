using UnityEngine;

public class Bubble : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
       

        if (other.CompareTag("Bubble"))
        {
            Debug.Log("a bubble!");
        }

        else
        {
            Debug.Log("something's broken");
        }
    }



}
