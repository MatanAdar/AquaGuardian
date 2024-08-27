using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Health : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Image healthBar;

    private float health = 100f;
    private float maxHealth = 100f;

    private float lerpSpeed;
    private float factorLerpSpeed = 3f;

    private float lifeTime; // wait this amout of time before dmg auto
    public TMP_InputField lifeTime_inputField;

    private float downHealthPairSec; // damage that happend every life time that we determand
    public TMP_InputField downHealthPairSec_inputField;

    [SerializeField] GameObject Panel;
    private bool moveOxygen = false; // Set to true by default

    public bool didntGetInputsYet = false;

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
        if (didntGetInputsYet)
        {
            // make speed. vertical speed and idle upward speed from user
            bool isLifeTimeValid = float.TryParse(lifeTime_inputField.text, out lifeTime);
            bool isDownHealthPairSecValid = float.TryParse(downHealthPairSec_inputField.text, out downHealthPairSec);
            if (isLifeTimeValid && isDownHealthPairSecValid)
            {
                lifeTime = float.Parse(lifeTime_inputField.text);
                downHealthPairSec = float.Parse(downHealthPairSec_inputField.text);
            }
            else
            {
                Debug.Log("error: " + lifeTime_inputField.text);
                Debug.Log("error: " + downHealthPairSec_inputField.text);
            }

            Debug.Log("lifeTime: " + lifeTime + ", downHealthPairSec: " + downHealthPairSec);

            didntGetInputsYet = false;
        }

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

        lerpSpeed = factorLerpSpeed * Time.deltaTime;
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
