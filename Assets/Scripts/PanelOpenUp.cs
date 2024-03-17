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
    public float num_caves_from_user = 0;
    [SerializeField] public Slider slider;
    private int pivotPlace = 50;
    private float chestX = 291.774f;
    private float chestY = 20.002f;

    public void num_of_caves(float value)
    {
        num_caves_from_user = 0;
        Debug.Log("before update: " + num_caves_from_user);

        // Round down the value to the nearest integer
        int intValue = Mathf.FloorToInt(value);

        num_of_caves_Text.text = intValue.ToString("0");

        num_caves_from_user = intValue;

        Debug.Log("num_caves_from_user after update: " + num_caves_from_user);
    }


    public void ClosePanel()
    {
        if (Panel != null)
        {
            Panel.SetActive(false);
            Debug.Log("num_caves_from_user in ClosePanel: " + num_caves_from_user);
            Vector3 currentPosition = objectToScale.transform.position;
            Vector3 newPosition = new Vector3(currentPosition.x,currentPosition.y,currentPosition.z);

            for (int i = 1; i <= num_caves_from_user; i++)
            {
                newPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - (pivotPlace * i));
                GameObject newObject = Instantiate(objectToScale, newPosition, Quaternion.identity);
            }

            
            Vector3 newPosition_chest = new Vector3(chestX, chestY, newPosition.z - (pivotPlace));

            GameObject newObject_chest = Instantiate(chest, newPosition_chest, Quaternion.identity);

        }
    }

}