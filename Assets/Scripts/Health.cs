using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEngine.Rendering.DebugUI;

public class Health : MonoBehaviour
{

    public TextMeshProUGUI healthText;
    public Image healthBar;

    float health = 100;
    float maxHealth = 100;

    float lerpSpeed;

    [SerializeField] float lifeTime;
    [SerializeField] float downHealthPairSec;

    public GameObject Panel;
    private bool moveOxygen = false; // Set to true by default

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        // Check if the panel is initially closed and start the coroutine if it is
        if (Panel != null && !Panel.activeSelf)
        {
            moveOxygen = true;
            StartCoroutine(DisappearHealthPoints());
        }
    }


    // Update is called once per frame
    void Update()
    {
        healthText.text = "Oxygen: " + health + "%";

        healthBarFiller();

        colorChanger();

        // Start the coroutine when the panel is closed
        if (Panel != null && !Panel.activeSelf && !moveOxygen)
        {
            moveOxygen = true;
            StartCoroutine(DisappearHealthPoints());
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        lerpSpeed = 3f * Time.deltaTime;
    }

    void healthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, lerpSpeed);
    }

    void colorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        healthBar.color = healthColor;
    }

    public void damage(float damagePoint)
    {
        if (health > damagePoint)
        {
            health -= damagePoint;
        }
        else
        {
            health = 0;
            gameOver();
        }
    }

    public void heal(float healingPoint)
    {
        if (health < maxHealth)
        {
            health += healingPoint;
        }
    }

    IEnumerator DisappearHealthPoints()
    {
        for (float i = health; i > 0; i--)
        {
            yield return new WaitForSeconds(lifeTime); // Wait for the specified lifetime

            damage(downHealthPairSec);

            // Check if all health points have disappeared
            if (i <= 0)
            {
                gameOver();
            }
        }
    }

    void gameOver()
    {
        SceneManager.LoadScene("Game_Over");
    }

}
