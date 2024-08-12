using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getEventFromAmadeoClientDiver : MonoBehaviour
{
    [SerializeField] private AmadeoClient amadeoClient;

    // Smoothing factor to control how quickly the object moves towards the target position
    public float smoothSpeed = 0.5f;

    public GameObject Panel;

    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;

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
        if (!Panel.activeSelf && forces != null && forces.Length > 0)
        {
            float forceValue = forces[3];
            
            // Move the fish based on forces[2]
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + (forceValue), transform.position.z);
            gameObject.transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed * Time.deltaTime);
            

        }
    }

}
