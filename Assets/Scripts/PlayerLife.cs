using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerLife : MonoBehaviour
{
    public int maxCollisions = 3; // Maximum number of collisions allowed
    private int currentCollisions = 0; // Current number of collisions
    private bool canCollide = true; // Flag to control collision timing
    private float waitTime = 2f;

    private bool PlayerisCollide = false;
    float PlayerPositionX;
    float PlayerPositionY;

    [SerializeField] public GameObject TopOfCave = null;
    [SerializeField] public GameObject BottomOfCave = null;

    float pivotToCaveCenter = 8f;

    /*public GameObject objectToDisappear1;
    public GameObject objectToDisappear2;
    public GameObject objectToDisappear3;*/

    [SerializeField] GameObject fish1;
    [SerializeField] GameObject fish2;
    [SerializeField] GameObject fish3;
    [SerializeField] GameObject fish4;

    public AudioClip collisionSound; // Assign this in the inspector
    private AudioSource audioSource;

    /*[SerializeField] GameObject healthBarObject;
    private HealthBar healthBar; // Reference to the HealthBar component*/

    [SerializeField] GameObject healthBarObject2;
    private Health healthBar2; // Reference to the HealthBar component

    [SerializeField] int removeHealthWithCollide;

    [SerializeField] int removeHealthFishCollide;

    [SerializeField] int timeBetweenCollides;

    float distance = 0;

    void Start()
    {
        PlayerPositionX= gameObject.transform.position.x;
        PlayerPositionY= (TopOfCave.transform.position.y + BottomOfCave.transform.position.y)/2;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = collisionSound;

        /*// Get the HealthBar component
        if (healthBarObject != null)
        {
            healthBar = healthBarObject.GetComponent<HealthBar>();
        }*/

        // Get the HealthBar component
        if (healthBarObject2 != null)
        {
            healthBar2 = healthBarObject2.GetComponent<Health>();
        }

        distance = gameObject.transform.position.z - fish1.transform.position.z;
    }

    private void Update()
    {
        float currentDistance = gameObject.transform.position.z - fish1.transform.position.z;
        if (distance != currentDistance)
        {
            Vector3 newPosition1 = fish1.transform.position;
            Vector3 newPosition2 = fish2.transform.position;
            Vector3 newPosition3 = fish3.transform.position;
            Vector3 newPosition4 = fish4.transform.position;
            
            newPosition1.z = gameObject.transform.position.z - distance;
            newPosition2.z = gameObject.transform.position.z - distance;
            newPosition3.z = gameObject.transform.position.z - distance;
            newPosition4.z = gameObject.transform.position.z - distance;

            fish1.transform.position = newPosition1;
            fish2.transform.position = newPosition2;
            fish3.transform.position = newPosition3;
            fish4.transform.position = newPosition4;
        }

        /*if (PlayerisCollide)
        {
            gameObject.transform.position = new Vector3(PlayerPositionX, PlayerPositionY, gameObject.transform.position.z);

            PlayerisCollide = false;
        }*/
    }

    void OnCollisionEnter(Collision collision)
    {
        if (canCollide && collision.collider.CompareTag("Cave"))
        {
            PlayerisCollide = true;

            HandleCollision();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cave"))
        {
            if (other.gameObject == fish1 || other.gameObject == fish2 || other.gameObject == fish3 || other.gameObject == fish4)
            {
                if (healthBar2 != null)
                {
                    healthBar2.damage(removeHealthFishCollide);
                }
            }
        }
    }

    private void HandleCollision()
    {
        currentCollisions++; // Increment collision count
        Debug.Log("adi_colosion");

        // Play collision sound
        PlayCollisionSound();

        // Remove health points
       /* if (healthBar != null)
        {
            healthBar.RemoveHealthPoints(removeWithCollide);
        }*/

        // Remove health points
        if (healthBar2 != null && canCollide)
        {
            healthBar2.damage(removeHealthWithCollide);

            StartCoroutine(Wait(timeBetweenCollides));
            
        }

        /*StartCoroutine(DisableObjectAndDelay(currentCollisions));*/
    }

    /*    IEnumerator DisableObjectAndDelay(int collisions)
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
                    // Call the GameOver method after 0.2f second delay
                    Invoke("GameOver", 0.2f);
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(waitTime); // Wait for 2 seconds
            canCollide = true; // Enable collision after delay
        }*/

    IEnumerator Wait(int number)
    {
        canCollide = false; // Disable collision temporarily
        yield return new WaitForSeconds(number); // Wait for 1 seconds
        canCollide = true; // Enable collision after delay
    }

    void GameOver()
    {
        // Load the Game Over scene
        SceneManager.LoadScene("Game_Over");
    }

    void PlayCollisionSound()
    {
        if (audioSource != null && collisionSound != null)
        {
            audioSource.Play();
        }
    }
}
