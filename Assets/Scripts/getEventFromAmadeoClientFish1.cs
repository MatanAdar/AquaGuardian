using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class getEventFromAmadeoClientFish1 : MonoBehaviour
{
    [SerializeField] private AmadeoClient amadeoClient;

    private ScenesManager scenesManager;

    // Smoothing factor to control how quickly the object moves towards the target position
    public float smoothSpeed = 0.5f;

    public GameObject Panel;

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
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (!Panel.activeSelf && forces != null && forces.Length > 0)
        {
            float forceValue = 0;

            if (sceneIndex == 4)
            {
                forceValue = forces[0];
            }
            else
            {
                forceValue = forces[4];
            }

            // Move the fish based on forces[0]
            Vector3 newPosition = new Vector3(transform.position.x,transform.position.y + (forceValue),transform.position.z);
            gameObject.transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed * Time.deltaTime);
        }
    }

}
