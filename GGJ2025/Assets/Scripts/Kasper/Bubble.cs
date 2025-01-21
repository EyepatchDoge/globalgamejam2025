using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    #region variables
    [SerializeField]
    FuelSystem fuelSystem;

    [SerializeField]
    float fuelAmount = 25f;
    [SerializeField]
    float hidetimer = 2.0f;
    #endregion

    void OnTriggerEnter(Collider other)
    {
       

        if (other.CompareTag("Bubble"))
        {
            Debug.Log("a bubble!");
            fuelSystem.AddFuel(fuelAmount);

            StartCoroutine(HideAndShow(hidetimer, other.gameObject));
        }

        else
        {
            Debug.Log("something's broken");
        }
    }
    private IEnumerator HideAndShow(float delay, GameObject bubble)
    {
        bubble.SetActive(false);
        yield return new WaitForSeconds(delay); // Wait for the specified time
        bubble.SetActive(true); // Reactivate the bubble GameObject after the delay
    }

}
