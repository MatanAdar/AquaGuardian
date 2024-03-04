using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CaveSpawner : MonoBehaviour
{
    [SerializeField] public GameObject objectToScale = null; // Reference to the object you want to scale
    [SerializeField] private float maxSliderAmount = 100.0f;
    [SerializeField] private float startPosition_Z = 400f; // Starting position of the object in the Z-axis
    [SerializeField] private float maxPosition_Z = 1100f; // Maximum position of the object in the Z-axis
    [SerializeField] private TextMeshProUGUI sliderText_pos_Z = null;

    private Vector3 positionOfSpawnedObject = Vector3.zero; // Position where the object will be spawned
    private GameObject spawnedObject; // Reference to the spawned object

    void Start()
    {
        // Instantiate the object and store the reference
        spawnedObject = Instantiate(objectToScale, positionOfSpawnedObject, Quaternion.identity);
    }

    public void SliderChange_pos_z(float value)
    {
        float scaledValue = value * maxSliderAmount;
        sliderText_pos_Z.text = scaledValue.ToString("0");

        // Calculate the new Z position based on the slider value
        float newZPosition = Mathf.Lerp(startPosition_Z, maxPosition_Z, scaledValue / maxSliderAmount);

        // Set the position where the object will be spawned
        positionOfSpawnedObject = new Vector3(0, 0, newZPosition);

        // Adjust the position of the object to reflect the change in spawn position
        spawnedObject.transform.position = positionOfSpawnedObject;
    }
}
