using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float verticalSpeed; // Adjust this for the speed of upward and downward movement
    public float idleUpwardSpeed; // Adjust this for the speed of upward movement when no input is detected

    private Rigidbody rb;
    public GameObject Panel;
    private bool canMove = true; // Set to true by default

    [SerializeField] public TextMeshProUGUI infoText; // Reference to the text object
    private bool show = true;

    [SerializeField] GameObject blue;
    [SerializeField] GameObject green;
    [SerializeField] GameObject red;
    [SerializeField] GameObject purple;

    private int counterFish = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Make sure to set the Rigidbody's collision detection mode to Continuous for accurate collision handling
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (infoText != null)
        {
            infoText.gameObject.SetActive(false); // Hide the text initially
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
        if (infoText != null)
        {
            infoText.gameObject.SetActive(true); // Show the text
            yield return new WaitForSeconds(4f); // Wait for 4 seconds
            infoText.gameObject.SetActive(false); // Hide the text after 4 seconds
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player has collided with the cave
        if (other.CompareTag("Cave") && blue != null)
        {
            Debug.Log("collide");
            // Disable the fish when colliding with the cave
            blue.SetActive(false);
        }
    }
}
