using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    float health = 100f;
    float maxHealth = 100f;
    float lerpSpeed = 0.1f;

    public Image[] healthPoints;
    public Image healthBar;

    int counter = 11;

    private bool[] healthPointStates; // To track which health points have disappeared

    [SerializeField] public float lifeTime;

    public GameObject Panel;
    private bool moveOxygen = false; // Set to true by default

    private void Start()
    {
        // Initialize the healthPointStates array
        healthPointStates = new bool[healthPoints.Length];
        for (int i = 0; i < healthPointStates.Length; i++)
        {
            healthPointStates[i] = true; // All points start as visible
        }

        // Check if the panel is initially closed and start the coroutine if it is
        if (Panel != null && !Panel.activeSelf)
        {
            moveOxygen = true;
            StartCoroutine(DisappearHealthPoints());
        }
    }

    private void Update()
    {
        healthBarFiller();

        // Start the coroutine when the panel is closed
        if (Panel != null && !Panel.activeSelf && !moveOxygen)
        {
            moveOxygen = true;
            StartCoroutine(DisappearHealthPoints());
        }
    }

    bool DisplayHealthPoint(float _health, int pointNumber)
    {
        return ((pointNumber * 10) >= _health);
    }

    void healthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (health / maxHealth), lerpSpeed);

        for (int i = 0; i < healthPoints.Length; i++)
        {
            if (healthPointStates[i]) // Only update if the point hasn't disappeared
            {
                healthPoints[i].enabled = !DisplayHealthPoint(health, i);
            }
        }
    }

    IEnumerator DisappearHealthPoints()
    {
        for (int i = 0; i < healthPoints.Length; i++)
        {
            yield return new WaitForSeconds(lifeTime); // Wait for the specified lifetime
            healthPoints[healthPoints.Length - counter + 1].enabled = false; // Disable the health point image
            healthPointStates[healthPoints.Length - counter + 1] = false; // Mark this health point as disappeared

            counter--;

            i = healthPoints.Length - counter + 1;

            // Check if all health points have disappeared
            if (i == healthPoints.Length - 1)
            {
                SceneManager.LoadScene("Game_Over");
            }
        }
    }

    public void AddHealthPoints(int points)
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (health / maxHealth), lerpSpeed);

        for (int j = 0; j < points; j++)
        {
            if (counter <= healthPoints.Length) // Only update if the point hasn't disappeared
            {
                healthPoints[healthPoints.Length - counter].enabled = true;

                counter++;
            }
        }
   
    }

    public void RemoveHealthPoints(int points)
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (health / maxHealth), lerpSpeed);

        for (int j = 0; j < points; j++)
        {
           
            healthPoints[healthPoints.Length - counter + 1].enabled = false; // Disable the health point image
            healthPointStates[healthPoints.Length - counter + 1] = false; // Mark this health point as disappeared

            counter--;

            j = healthPoints.Length - counter + 1;

            // Check if all health points have disappeared
            if (j == healthPoints.Length - 1)
            {
                SceneManager.LoadScene("Game_Over");
            }
        }

    }
}
