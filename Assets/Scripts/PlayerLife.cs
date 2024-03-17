using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerLife : MonoBehaviour
{
    public int maxCollisions = 3; // Maximum number of collisions allowed
    private int currentCollisions = 0; // Current number of collisions
    private bool canCollide = true; // Flag to control collision timing

    public GameObject objectToDisappear1;
    public GameObject objectToDisappear2;
    public GameObject objectToDisappear3;

    void OnCollisionEnter(Collision collision)
    {
        if (canCollide && collision.collider.CompareTag("Cave"))
        {
            currentCollisions++; // Increment collision count
            Debug.Log("adi_colosion");

           /* if (currentCollisions >= maxCollisions)
            {
                
                // Call the GameOver method after 1 second delay
                Invoke("GameOver", 1f);
            }
            else
            {*/
                StartCoroutine(DisableObjectAndDelay(currentCollisions));
            
        }
    }

    IEnumerator DisableObjectAndDelay(int collisions)
    {
        canCollide = false; // Disable collision temporarily
        // Determine which object to disappear based on currentCollisions value
        switch (collisions)
        {
            case 1:
                if (objectToDisappear1 != null)
                    objectToDisappear1.SetActive(false);
                break;
            case 2:
                if (objectToDisappear2 != null)
                    objectToDisappear2.SetActive(false);
                break;
            case 3:
                if (objectToDisappear3 != null)
                    objectToDisappear3.SetActive(false);
                    // Call the GameOver method after 1 second delay
                    Invoke("GameOver", 0.2f);
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        canCollide = true; // Enable collision after delay
    }

    void GameOver()
    {
        // Load the Game Over scene
        SceneManager.LoadScene("Game_Over");
    }
}
