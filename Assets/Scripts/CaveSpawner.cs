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
    private Vector3 velocityOfSpawnedObject = Vector3.zero; // Velocity of the spawned object

    void Start()
    {
        // Start the SpawnRoutine coroutine
        this.StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // Spawn the object with the parameters set by the player
            GameObject newObject = Instantiate(objectToScale, positionOfSpawnedObject, Quaternion.identity);

            // Wait for a certain amount of time before spawning the next object
            yield return new WaitForSeconds(1.0f);
        }
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
        objectToScale.transform.position = positionOfSpawnedObject;
    }
}
