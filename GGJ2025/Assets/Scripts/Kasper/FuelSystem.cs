using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class FuelSystem : MonoBehaviour
{

    public float currentFuel = 100f;
    public float maxFuel = 100f;

    public TextMeshProUGUI fuelDisplay;
    public Slider fuelslider;

    
    public Volume redOverlay;
    public float redVolumeMaxIntensity = 0.5f;

    public AnimationCurve redOverlayCurve;

    private void Start()
    {
        UpdateFuel();

    }

    private void Update()
    {
        float fuelpercentage = GetFuelAsPercentage();
        float volumeIntensity = redOverlayCurve.Evaluate(fuelpercentage);
        redOverlay.weight = volumeIntensity;
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

    float GetFuelAsPercentage()
    {
        return Mathf.InverseLerp(0, maxFuel, currentFuel);
    }

}
