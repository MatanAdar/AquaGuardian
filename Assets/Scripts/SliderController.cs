using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliderController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sliderText = null;
    [SerializeField] public GameObject objectToScale = null; // Reference to the object you want to scale
    [SerializeField] private float maxSliderAmount = 100.0f;
    [SerializeField] private Vector3 maxScale = new Vector3(2f, 2f, 2f); // Maximum scale of the object

    public void SliderChange(float value)
    {
        float scaledValue = value * maxSliderAmount;
        sliderText.text = scaledValue.ToString("0");

        // Calculate the new scale based on the slider value
        Vector3 newScale = Vector3.Lerp(Vector3.one, maxScale, scaledValue / maxSliderAmount);

        // Apply the new scale to the object
        objectToScale.transform.localScale = newScale;
    }
}
