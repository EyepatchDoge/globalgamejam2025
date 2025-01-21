using UnityEngine;

public class Bubble : MonoBehaviour
{

    [SerializeField]
    FuelSystem fuelSystem;

    [SerializeField]
    float fuelAmount = 25f;

    void OnTriggerEnter(Collider other)
    {
       

        if (other.CompareTag("Bubble"))
        {
            Debug.Log("a bubble!");
            fuelSystem.AddFuel(fuelAmount);
        }

        else
        {
            Debug.Log("something's broken");
        }
    }



}
