/*using UnityEngine;
using System.IO;

public class ReadCSV : MonoBehaviour
{
    public string filePath = "Assets\\Data\\caves.csv";

    void Start()
    {
        ReadCSVFile(filePath);
    }

    void ReadCSVFile(string path)
    {
        try
        {
            string[] lines = File.ReadAllLines(path);
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
}*/