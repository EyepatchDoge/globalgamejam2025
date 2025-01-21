using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class FuelSystem : MonoBehaviour
{

    public float currentFuel = 100f;
    public float maxFuel = 100f;

    public TextMeshProUGUI fuelDisplay;
    public Slider fuelslider;

    private void Start()
    {
        UpdateFuel();

    }

    public void UpdateFuel()
    {
        fuelDisplay.text = "Fuel Left:" + currentFuel.ToString();
        fuelslider.value = currentFuel/maxFuel;
    }

    public void UseFuel(float fuelUsed)
    {
        currentFuel = Mathf.Max(currentFuel - fuelUsed, 0);
        UpdateFuel();
    }

    public void AddFuel(float amount)
    {
        currentFuel = Mathf.Min(currentFuel + amount, maxFuel);
        UpdateFuel() ;
    }
}
