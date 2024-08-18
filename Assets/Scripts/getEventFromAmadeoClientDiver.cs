using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


public class getEventFromAmadeoClientDiver : MonoBehaviour
{
    [SerializeField] private AmadeoClient amadeoClient;
    [SerializeField] float factor_forces = 10f;

    // Smoothing factor to control how quickly the object moves towards the target position
    private float smoothSpeed = 1.5f;

    [SerializeField] float verticalTolerance = 0.1f;

    [SerializeField] GameObject Panel;

    private int indexForce = -1;

    public TMP_InputField factor_force_inputField;
    private Rigidbody rb;
    private PlayerMovement pm;

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        // Make sure to set the Rigidbody's collision detection mode to Continuous for accurate collision handling
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        pm = GetComponent<PlayerMovement>();
    }


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

        if (pm.canMove && pm.afterText)
        {
            if (!Panel.activeSelf && forces != null && forces.Length > 0)
            {
                Vector3 movementDirection = Vector3.forward; // Move along the z-axis (forward direction)
                Vector3 targetVelocity = pm.speed * transform.TransformDirection(movementDirection);

                Debug.Log("factor_force: " + float.Parse(factor_force_inputField.text));
                float newVerticalPosition = forces[indexForce] * float.Parse(factor_force_inputField.text); // Apply factor here
                float currentVerticalPosition = transform.position.y;
                float verticalMovementSpeed;
                if (Mathf.Abs(newVerticalPosition - currentVerticalPosition) < verticalTolerance)
                {
                    //verticalMovementSpeed = 0;
                    verticalMovementSpeed = pm.idleUpwardSpeed;
                } else {
                    verticalMovementSpeed = Mathf.Sign(newVerticalPosition - currentVerticalPosition) * pm.verticalSpeed;
                }
                //   float verticalMovementSpeed = (newVerticalPosition - currentVerticalPosition) / 0.5f; //forceValue * pm.verticalSpeed;
                /*if (verticalMovementSpeed <= 0)
                {
                    verticalMovementSpeed += pm.idleUpwardSpeed;
                }*/
                targetVelocity += verticalMovementSpeed * transform.TransformDirection(Vector3.up);
                Debug.Log("targetVelocity = " + targetVelocity);
                rb.velocity = targetVelocity;
            }
        }

        /*
        if (!Panel.activeSelf && forces != null && forces.Length > 0)
        {
            Debug.Log("factor_force: " + float.Parse(factor_force_inputField.text));

            float forceValue = forces[indexForce] * float.Parse(factor_force_inputField.text); // Apply factor here

            float verticalMovementSpeed = forceValue * smoothSpeed;
            // Calculate the target position
                        Vector3 targetPosition = new Vector3(
                            transform.position.x,
                            transform.position.y + forceValue,
                            transform.position.z
                        );
            
            // Lerp towards the target position smoothly
            // transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }*/
    }

}
