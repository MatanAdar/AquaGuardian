using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class getEventFromAmadeoClientDiver : MonoBehaviour
{
    [SerializeField] private AmadeoClient amadeoClient;
    [SerializeField] float factor_forces = 10f;

    // Smoothing factor to control how quickly the object moves towards the target position
    private float smoothSpeed = 1.5f;

    [SerializeField] GameObject Panel;

    private int indexForce = -1;

    public TMP_InputField factor_force_inputField;


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

    public void SelectFinger(int fingerIndex)
    {
        indexForce = fingerIndex;
    }


    private void HandleForcesUpdated(float[] forces)
    {
        Debug.Log(indexForce);

        if (!Panel.activeSelf && forces != null && forces.Length > 0)
        {
            Debug.Log("factor_force: " + float.Parse(factor_force_inputField.text));

            float forceValue = forces[indexForce] * float.Parse(factor_force_inputField.text); // Apply factor here

            // Calculate the target position
            Vector3 targetPosition = new Vector3(
                transform.position.x,
                transform.position.y + forceValue,
                transform.position.z
            );

            // Lerp towards the target position smoothly
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }

}
