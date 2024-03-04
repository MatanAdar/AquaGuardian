using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float verticalSpeed; // Adjust this for the speed of upward and downward movement
    public LayerMask groundMask;

    private CharacterController characterController;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(transform.position, characterController.radius, groundMask);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float upDownInput = Input.GetAxis("UpDown");

        Vector3 movementDirection = new Vector3(horizontalInput, upDownInput, verticalInput); // Allow vertical input for movement
        movementDirection.Normalize();

        // Apply movement
        Vector3 move = transform.TransformDirection(movementDirection) * speed * Time.deltaTime;
        characterController.Move(move);

        // Apply vertical movement directly
        float verticalMovement = upDownInput * verticalSpeed * Time.deltaTime;
        characterController.Move(Vector3.up * verticalMovement);

        // Rotate towards movement direction
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }


}
