using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelOpenUp : MonoBehaviour
{
    public GameObject Panel;
    [SerializeField] public float maxSliderAmount = 5.0f;
    [SerializeField] public GameObject objectToScale = null;
    [SerializeField] public TextMeshProUGUI num_of_caves_Text = null;
    public static float num_caves_from_user = 1;
    [SerializeField] public Slider slider;

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
            for (int i = 1; i < num_caves_from_user; i++)
            {
                Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y, (160 * i) + currentPosition.z);
                GameObject newObject = Instantiate(objectToScale, newPosition, Quaternion.identity);
            }
        }
    }
}
