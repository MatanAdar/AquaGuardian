using UnityEngine;
using TMPro;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float verticalSpeed; // Adjust this for the speed of upward and downward movement
    public float idleUpwardSpeed; // Adjust this for the speed of upward movement when no input is detected

    private Rigidbody rb;
    public GameObject Panel;
    private bool canMove = true; // Set to true by default

    [SerializeField] public TextMeshProUGUI infoText1; // Reference to the text object
    [SerializeField] public TextMeshProUGUI infoText2; // Reference to the text object
    [SerializeField] public TextMeshProUGUI infoText3; // Reference to the text object
    [SerializeField] public TextMeshProUGUI infoText4; // Reference to the text object
    private bool show = true;

    [SerializeField] GameObject blue;
    [SerializeField] GameObject green;
    [SerializeField] GameObject red;
    [SerializeField] GameObject purple;

    private int counterFish = 0;

    private bool canCollide = true;
    public float collisionDelay = 2f;

    [SerializeField] string sceneName;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Make sure to set the Rigidbody's collision detection mode to Continuous for accurate collision handling
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (infoText1 != null && infoText2 != null && infoText3 != null && infoText4 != null)
        {
            infoText1.gameObject.SetActive(false); // Hide the text initially
            infoText2.gameObject.SetActive(false); // Hide the text initially
            infoText3.gameObject.SetActive(false); // Hide the text initially
            infoText4.gameObject.SetActive(false); // Hide the text initially
        }
    }

    void Update()
    {
        // Check if the panel is active and set canMove accordingly
        if (Panel != null)
        {
            canMove = !Panel.activeSelf;
        }

        if (canMove)
        {
            if (show)
            {
                // Show the info text for 4 seconds
                StartCoroutine(ShowInfoText());
                show = false;
            }

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            float upDownInput = Input.GetAxis("UpDown");

            Vector3 movementDirection = new Vector3(horizontalInput, upDownInput, verticalInput); // Allow vertical input for movement
            movementDirection.Normalize();

            // Apply movement
            Vector3 move = transform.TransformDirection(movementDirection) * speed * Time.deltaTime;
            rb.MovePosition(rb.position + move);

            // Apply vertical movement directly
            float verticalMovement = upDownInput * verticalSpeed * Time.deltaTime;

            // If no down input, apply idle upward movement
            if (upDownInput <= 0)
            {
                verticalMovement += idleUpwardSpeed * Time.deltaTime;
            }

            rb.MovePosition(rb.position + Vector3.up * verticalMovement);

            // Rotate towards movement direction
            if (movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, toRotation, rotationSpeed * Time.deltaTime));
            }
        }
    }

    // Method to toggle player movement on or off
    public void ToggleMovement(bool move)
    {
        canMove = move;
    }

    private IEnumerator ShowInfoText()
    {
        if (infoText1 != null && infoText2 != null && infoText3 != null && infoText4 != null)
        {
            infoText1.gameObject.SetActive(true); // Show the text
            infoText2.gameObject.SetActive(true); // Show the text
            yield return new WaitForSeconds(5f); // Wait for 4 seconds
            infoText1.gameObject.SetActive(false); // Hide the text after 4 seconds
            infoText2.gameObject.SetActive(false); // Hide the text after 4 seconds

            yield return new WaitForSeconds(0.5f);

            infoText3.gameObject.SetActive(true); // Show the text
            yield return new WaitForSeconds(4f);
            infoText3.gameObject.SetActive(false); // Show the text

            yield return new WaitForSeconds(0.5f);

            infoText4.gameObject.SetActive(true); // Show the text
            yield return new WaitForSeconds(4f);
            infoText4.gameObject.SetActive(false); // Hide the text after 4 seconds
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player has collided with the cave
        if (collision.collider.CompareTag("Cave") && blue != null && counterFish == 0)
        {
            Debug.Log("Collide");
            // Disable the fish when colliding with the cave
            blue.SetActive(false);
            counterFish++;
            StartCoroutine(DisableFishAndDelay());
        }
        else if (collision.collider.CompareTag("Cave") && purple != null && counterFish == 1 && canCollide)
        {
            Debug.Log("Collide");
            // Disable the fish when colliding with the cave
            purple.SetActive(false);
            counterFish++;
            StartCoroutine(DisableFishAndDelay());
        }
        else if (collision.collider.CompareTag("Cave") && green != null && counterFish == 2 && canCollide)
        {
            Debug.Log("Collide");
            // Disable the fish when colliding with the cave
            green.SetActive(false);
            counterFish++;
            StartCoroutine(DisableFishAndDelay());
        }
        else if (collision.collider.CompareTag("Cave") && red != null && counterFish == 3 && canCollide)
        {
            Debug.Log("Collide");
            // Disable the fish when colliding with the cave
            red.SetActive(false);
            counterFish++;
            SceneManager.LoadScene(sceneName);

        }
    }

    private IEnumerator DisableFishAndDelay() {
        canCollide = false; // Prevent further collisions temporarily
        yield return new WaitForSeconds(collisionDelay);
        canCollide = true; // Allow collisions again after the delay
    }
}
