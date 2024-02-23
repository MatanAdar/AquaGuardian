using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishDisappeared1 : MonoBehaviour
{
    private bool keyHeld = false;
    private float keyPressedTime = 0f;
    public float timeThreshold = 4f; // 4 seconds threshold

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.H))
        {
            keyHeld = true;
            keyPressedTime = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.Y) || Input.GetKeyUp(KeyCode.H))
        {
            keyHeld = false;
        }

        if (keyHeld && Time.time - keyPressedTime >= timeThreshold)
        {
            gameObject.SetActive(false); // Disables the game object
            keyHeld = false; // Reset the key held status
        }
    }
}
