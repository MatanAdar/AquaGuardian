using UnityEngine;
using TMPro;

public class FishMover3 : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float verticalMoveSpeedUp;
    [SerializeField] float verticalMoveSpeedDown;
    public float maxOffsetUp; // Maximum offset from player's position
    public float maxOffsetDown;
    public float stateY;
    bool isSpacePressed = false;
    private bool canMove = true;
    public float statePlaterY;
    public GameObject Panel;
    private float pivotUp = 7f;
    private float fishKeepUpWithPlayer = 2f;
    private float pivot = 2f;
    private float minFontSize = 40f;
    private float maxFontSize = 80f;
    private float fontSizeIncrement = 5f;

    [SerializeField] public TextMeshProUGUI ButtonToPressUp; // Reference to the text object
    [SerializeField] public TextMeshProUGUI ButtonToPressDown; // Reference to the text object

    public void SetMaxOffsetUp(float Up)
    {
        this.maxOffsetUp = Up;
    }

    Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        stateY = transform.position.y;
        statePlaterY = player.transform.position.y;

        if (ButtonToPressUp != null && ButtonToPressDown != null)
        {
            ButtonToPressUp.gameObject.SetActive(false); // Hide the text initially
            ButtonToPressDown.gameObject.SetActive(false); // Hide the text initially
        }

        // Offset the text positions
        if (ButtonToPressUp != null)
        {
            Vector3 upTextPosition = transform.position + new Vector3(0f, 1f, 0f); // Offset up by 1 unit
            ButtonToPressUp.rectTransform.position = Camera.main.WorldToScreenPoint(upTextPosition);
        }

        if (ButtonToPressDown != null)
        {
            Vector3 downTextPosition = transform.position - new Vector3(0f, 1f, 0f); // Offset down by 1 unit
            ButtonToPressDown.rectTransform.position = Camera.main.WorldToScreenPoint(downTextPosition);
        }

        // Set initial target position
        SetNewTargetPosition();

    }

    // Set a new random target position within the defined range
    void SetNewTargetPosition()
    {
        // Move the fish downwards continuously if the space key is pressed
        if (isSpacePressed)
        {
            maxOffsetUp = 0f;
            MoveDown();
        }
        else
        {
            maxOffsetUp = 0f;
            MoveUp();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Panel != null)
        {
            canMove = !Panel.activeSelf;
        }
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Set the space key pressed flag to true
                isSpacePressed = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                // Reset the space key pressed flag to false
                isSpacePressed = false;
            }

            // Move towards the target position
            float horizontalMoveSpeed = player.GetComponent<PlayerMovement>().speed;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, fishKeepUpWithPlayer * horizontalMoveSpeed * Time.deltaTime);

            // Toggle text visibility based on fish position
            if (gameObject.activeInHierarchy)
            {
                if (gameObject.transform.position.y - pivot < player.transform.position.y)
                {
                    ButtonToPressDown.gameObject.SetActive(false);
                    ButtonToPressUp.gameObject.SetActive(true);
                    Vector3 upTextPosition = transform.position + new Vector3(0f, fishKeepUpWithPlayer * horizontalMoveSpeed * Time.deltaTime, 0f); // Offset up by 1 unit
                    ButtonToPressUp.rectTransform.position = Camera.main.WorldToScreenPoint(upTextPosition);
                }
                else if (gameObject.transform.position.y + pivot > player.transform.position.y)
                {
                    ButtonToPressUp.gameObject.SetActive(false);
                    ButtonToPressDown.gameObject.SetActive(true);
                    Vector3 downTextPosition = transform.position - new Vector3(0f, fishKeepUpWithPlayer * horizontalMoveSpeed * Time.deltaTime, 0f); // Offset down by 1 unit
                    ButtonToPressDown.rectTransform.position = Camera.main.WorldToScreenPoint(downTextPosition);
                }

                // Calculate the distance between the fish and the player along the Y-axis
                float distanceToPlayer = Mathf.Abs(transform.position.y - player.transform.position.y);

                // Adjust font size based on the distance
                AdjustTextSize(distanceToPlayer);
            }
            else
            {
                // Fish is inactive, hide the text
                HideText();
            }

            // If reached the target position, set a new target position
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f) // Check distance within a threshold
            {
                SetNewTargetPosition();
            }
        }
        // Check for vertical movement input
        CheckVerticalMovementInput();
    }

    // Check for vertical movement input
    void CheckVerticalMovementInput()
    {
        // Move down when 'J' is pressed
        if (Input.GetKeyDown(KeyCode.J))
        {
            MoveDown();
        }

        // Move up when 'U' is pressed
        if (Input.GetKeyDown(KeyCode.U))
        {
            MoveUp();
        }
    }

    // Move the fish down
    void MoveDown()
    {
        targetPosition = new Vector3(transform.position.x, transform.position.y - verticalMoveSpeedDown, player.transform.position.z);
    }

    // Move the fish up
    void MoveUp()
    {
        targetPosition = new Vector3(transform.position.x, Mathf.Min(player.transform.position.y + pivotUp, transform.position.y + verticalMoveSpeedUp), player.transform.position.z); ;
    }

    // Hide the text objects
    void HideText()
    {
        ButtonToPressDown.gameObject.SetActive(false);
        ButtonToPressUp.gameObject.SetActive(false);
    }

    // Show the text objects
    void ShowText()
    {
        ButtonToPressDown.gameObject.SetActive(true);
        ButtonToPressUp.gameObject.SetActive(true);
    }

    // Adjust text size based on the distance between fish and player
    void AdjustTextSize(float distanceToPlayer)
    {
        // Calculate font size based on distance
        float fontSize = minFontSize + (distanceToPlayer * fontSizeIncrement);

        // Clamp font size between min and max values
        fontSize = Mathf.Clamp(fontSize, minFontSize, maxFontSize);

        // Apply the calculated font size to the text objects
        ButtonToPressUp.fontSize = fontSize;
        ButtonToPressDown.fontSize = fontSize;
    }

    // Handle visibility of text when the fish becomes active
    void OnEnable()
    {
        ShowText();
    }

    // Handle visibility of text when the fish becomes inactive
    void OnDisable()
    {
        HideText();
    }

    // Collision with cave
    void OnCollisionEnter(Collision collision)
    {
        // Check if the player has collided with the cave
        if (collision.collider.CompareTag("Cave"))
        {
            Debug.Log("Collide");
            // Disable the fish when colliding with the cave
            gameObject.SetActive(false);
        }
    }
}
