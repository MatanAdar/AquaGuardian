using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getEventFromAmadeoClientFish4 : MonoBehaviour
{
    [SerializeField] private AmadeoClient amadeoClient;

    // Smoothing factor to control how quickly the object moves towards the target position
    public float smoothSpeed = 0.5f;

    public GameObject Panel;

    [SerializeField] public GameObject player = null;

    private float minPositionY = 0;
    private float maxPositionY = 0;

    private void OnEnable()
    {
        if (amadeoClient != null)
        {
            amadeoClient.OnForcesUpdated += HandleForcesUpdated;
        }
    }

    private void OnDisable()
    {
        if (amadeoClient != null)
        {
            amadeoClient.OnForcesUpdated -= HandleForcesUpdated;
        }
    }

    private void HandleForcesUpdated(float[] forces)
    {

        minPositionY = player.transform.position.y - (float)1.1;

        maxPositionY = player.transform.position.y + (float)1.1;

        if (!Panel.activeSelf && forces != null && forces.Length > 0)
        {
            float forceValue = forces[3];

            if (transform.position.y + forceValue > minPositionY && transform.position.y + forceValue < maxPositionY)
            {
                Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + (forceValue), transform.position.z);
                gameObject.transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed * Time.deltaTime);
            }
            else if (transform.position.y + forceValue < minPositionY)
            {
                Vector3 newPosition = new Vector3(transform.position.x, minPositionY, transform.position.z);
                gameObject.transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed * Time.deltaTime);
            }
            else if (transform.position.y + forceValue > maxPositionY)
            {
                Vector3 newPosition = new Vector3(transform.position.x, maxPositionY, transform.position.z);
                gameObject.transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed * Time.deltaTime);
            }

            /*// Move the fish based on forces[3]
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + (forceValue), transform.position.z);
            gameObject.transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed * Time.deltaTime);*/
        }
    }

}
