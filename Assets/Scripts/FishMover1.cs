using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class FishMover1 : MonoBehaviour
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
            
            // targetPosition = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);

            MoveUp();
        }
    }



    /* if (maxOffsetUp != 0)
     {


         // Calculate new maxY and minY based on player's position
         float maxY = player.transform.position.y + maxOffsetUp;
         float minY = player.transform.position.y - maxOffsetDown;
         float randomY;

         // float randomYU = Random.Range(stateY, maxY);
          float randomYD = Random.Range(minY, stateY);//

         // Check if the user presses the space key

         // If space key is pressed, use randomYU


         randomY = Random.Range(minY, maxY);
         //   while (maxOffsetUp == 0)
            {
                MoveDown();
               // randomY = 0;
             //   SetMaxOffsetUp()
            }//


         // If space key is not pressed, use randomYD
         //  randomY = Random.Range(player.transform.position.y, );



         targetPosition = new Vector3(transform.position.x, randomY, player.transform.position.z);


     }
*/


    // Start a coroutine to hold the target position for 3 seconds



    // Coroutine to hold the target position for 3 seconds



    /*        targetPosition = new Vector3(transform.position.x, randomY, player.transform.position.z);

            targetPosition = new Vector3(transform.position.x, randomY, player.transform.position.z);

            targetPosition = new Vector3(transform.position.x, randomY, player.transform.position.z);
            targetPosition = new Vector3(transform.position.x, randomY, player.transform.position.z);
            targetPosition = new Vector3(transform.position.x, randomY, player.transform.position.z);*/





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
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 2 * horizontalMoveSpeed * Time.deltaTime);

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




        // Move down when 'G' is pressed
        if (Input.GetKeyDown(KeyCode.G))
        {
            MoveDown();
        }

        // Move up when 'T' is pressed
        if (Input.GetKeyDown(KeyCode.T))
        {
            MoveUp();
        }
    }

    // Move the fish down
    void MoveDown()
    {

        targetPosition = new Vector3(transform.position.x, transform.position.y - verticalMoveSpeedDown, player.transform.position.z);
        /*   -verticalMoveSpeed*/
    }

    // Move the fish up
    void MoveUp()
    {/*
        targetPosition = player.transform.position;
        +verticalMoveSpeed*/
        targetPosition = new Vector3(transform.position.x, Mathf.Min(player.transform.position.y+ 3.5f, transform.position.y + verticalMoveSpeedUp), player.transform.position.z); ;
    }


    // Collison with cave
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