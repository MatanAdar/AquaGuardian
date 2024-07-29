using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public AmadeoClient amadeoClient; // Reference to your AmadeoClient script

    void Start()
    {
        // Get the Button component attached to the GameObject this script is attached to
        Button button = GetComponent<Button>();
        // Add a listener to the button to call the HandleButtonClick method when clicked
        button.onClick.AddListener(HandleButtonClick);
    }

    void HandleButtonClick()
    {
        // Call the StartReceiveData method with zeroF set to true
        amadeoClient.StartReceiveData(true);
    }
}
