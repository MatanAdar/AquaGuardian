using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using Unity.Mathematics;
using System.IO;

public class PanelOpenUp : MonoBehaviour
{
    public GameObject Panel;
    [SerializeField] public float maxSliderAmount = 5.0f;
    [SerializeField] public GameObject objectToScale = null;
    [SerializeField] public GameObject oxygenObject = null;
    [SerializeField] public GameObject chest = null;
    [SerializeField] public TextMeshProUGUI num_of_caves_Text = null;
    public float num_caves_from_user = 0;
    [SerializeField] public Slider slider;
    private int pivotPlace = 50;
    private float chestX = 291.774f;
    private float chestY = 20.002f;

    private int numOfLines = 0;

    public string filePath = "Assets\\Data\\caves.csv";

    string[] lines = null;

    void Start()
    {
        ReadCSVFile(filePath);
    }

    void ReadCSVFile(string path)
    {
        try
        {
            lines = File.ReadAllLines(path);

            numOfLines = lines.Length;

            Debug.Log("num of caves from file: " + numOfLines);

            foreach (string line in lines)
            {
                
                Debug.Log(line); // Prints each line of the CSV file

                // Split the line into fields based on the comma delimiter
                string[] fields = line.Split(',');

                // Process the fields as needed
                foreach (string field in fields)
                {
                    Debug.Log(field); // Prints each field in the current line
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Error reading file: {e.Message}");
        }
    }

    public void num_of_caves(float value)
    {
        num_caves_from_user = 0;
        Debug.Log("before update: " + num_caves_from_user);

        // Round down the value to the nearest integer
        int intValue = Mathf.FloorToInt(value);

        num_of_caves_Text.text = intValue.ToString("0");

        num_caves_from_user = intValue;

        Debug.Log("num_caves_from_user after update: " + numOfLines);
    }


    public void ClosePanel()
    {
        if (Panel != null)
        {
            Panel.SetActive(false);
            Debug.Log("num_caves_from_user in ClosePanel: " + numOfLines);
            Vector3 currentPosition = objectToScale.transform.position;
            Vector3 currentPositionOxygen = oxygenObject.transform.position;
            Vector3 newPosition = new Vector3(currentPosition.x,currentPosition.y,currentPosition.z);
            Vector3 newOxygenPosition = new Vector3(currentPositionOxygen.x, currentPositionOxygen.y, currentPositionOxygen.z);

            Vector3 currentScale = objectToScale.transform.localScale;
            Vector3 newScale = new Vector3(currentScale.x, currentScale.y, currentScale.z);

            //Category
            for (int j=1; j<=1; j++)
            {
                string[] fields = lines[j].Split(',');

                //For each row
                for (int i = 1; i <= numOfLines; i++)
                {
                    newPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - (pivotPlace * i));

                    float valueY = float.Parse(fields[i]);
                    Debug.Log("Y from file: " + valueY);
                    newScale = new Vector3(newScale.x, valueY, newScale.z);

                    newOxygenPosition = new Vector3(currentPositionOxygen.x, currentPositionOxygen.y, currentPositionOxygen.z - (pivotPlace * i));

                    //instantiate objects
                    GameObject newObject = Instantiate(objectToScale, newPosition, Quaternion.identity);
                    newObject.transform.localScale = newScale;
                    GameObject newOxygenObject = Instantiate(oxygenObject, newOxygenPosition, Quaternion.identity);
                }
            }

            
            Vector3 newPosition_chest = new Vector3(chestX, chestY, newPosition.z - (pivotPlace));

            GameObject newObject_chest = Instantiate(chest, newPosition_chest, Quaternion.identity);

        }
    }

   /* public void num_of_caves(float value)
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
            Vector3 currentPositionOxygen = oxygenObject.transform.position;
            Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z);
            Vector3 newOxygenPosition = new Vector3(currentPositionOxygen.x, currentPositionOxygen.y, currentPositionOxygen.z);

            for (int i = 1; i <= num_caves_from_user; i++)
            {
                newPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - (pivotPlace * i));
                newOxygenPosition = new Vector3(currentPositionOxygen.x, currentPositionOxygen.y, currentPositionOxygen.z - (pivotPlace * i));
                GameObject newObject = Instantiate(objectToScale, newPosition, Quaternion.identity);
                GameObject newOxygenObject = Instantiate(oxygenObject, newOxygenPosition, Quaternion.identity);
            }


            Vector3 newPosition_chest = new Vector3(chestX, chestY, newPosition.z - (pivotPlace));

            GameObject newObject_chest = Instantiate(chest, newPosition_chest, Quaternion.identity);

        }
    }*/

}