using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using Unity.Mathematics;


public class PanelOpenUp : MonoBehaviour
{
    public GameObject Panel;
    [SerializeField] public float maxSliderAmount = 5.0f;
    [SerializeField] public GameObject objectToScale = null;
    [SerializeField] public GameObject chest = null;
    [SerializeField] public TextMeshProUGUI num_of_caves_Text = null;
    public static float num_caves_from_user = 1;
    [SerializeField] public Slider slider;
    private int pivotPlace = 50;
    private int pivotChest = 70;
    private float chestX = 291.774f;
    private float chestY = 20.002f;

    public void num_of_caves(float value)
    {
        Debug.Log("before update: " + num_caves_from_user);
        float scaledValue = value;
        num_of_caves_Text.text = scaledValue.ToString("0");

        num_caves_from_user = value;
        Debug.Log("num_caves_from_user after update: " + num_caves_from_user);
    }

    public void ClosePanel()
    {
        if (Panel != null)
        {
            Panel.SetActive(false);
            Debug.Log("num_caves_from_user in ClosePanel: " + num_caves_from_user);
            Vector3 currentPosition = objectToScale.transform.position;
            float totalCavesLength = pivotPlace * (num_caves_from_user); // Total length covered by all caves except the last one

            for (int i = 1; i < num_caves_from_user; i++)
            {
                Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - (pivotPlace * i));
                GameObject newObject = Instantiate(objectToScale, newPosition, Quaternion.identity);
            }

            // Calculate the position of the treasury after all the caves
            float chestOffset = 40.0f; // Adjust this value as needed
            Vector3 newPosition_chest = new Vector3(chestX, chestY, currentPosition.z - totalCavesLength - math.pow(chestOffset, num_caves_from_user - 1));

            GameObject newObject_chest = Instantiate(chest, newPosition_chest, Quaternion.identity);

        }
    }

}