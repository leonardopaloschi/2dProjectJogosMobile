using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealthPoints = 100f;
    public float healthPoints;
    public TMP_Text healthTxt;
    public Slider slider;
    public GameObject gameOverPanel;
    private bool canTakeDamage = true;

    public adManager am;

    

    void Awake()
    {
        if (am == null)
        {
            am = FindObjectOfType<adManager>();
            if (am == null)
            {
                Debug.LogError("adManager reference not set on Health script and could not be found in the scene.");
            }
        }
    }

    private float damageCooldown = 1f;

    public bool isKnockbackActive = false;
    public float knockbackAmount;

    void Start()
    {
        healthPoints = maxHealthPoints;
    }

    public void ResetHealth()
    {
        
        healthPoints = maxHealthPoints;
        HandleMudarSlider(healthPoints);
        slider.value = healthPoints;
        canTakeDamage = true;
        damageCooldown = 1f; // Reset cooldown when health is reset
        


    }

    void Update()
    {
        if (damageCooldown > 0)
        {
            damageCooldown -= Time.deltaTime;
        }
        else
        {
            damageCooldown = 0;
        }
    }

    public void HandleMudarSlider(float valor)
    {
        if (healthTxt != null)
        {
            healthTxt.SetText(valor.ToString("F0"));
        }
    }

    public void TomarDano(float dano)
    {
        Debug.Log("Tomando dano: " + dano);
        Debug.Log("vida atual: " + healthPoints);
        if (damageCooldown == 0 && canTakeDamage)
        {
            healthPoints -= dano;

            if (healthPoints <= 0)
            {
                healthPoints = 0;
                //SceneManager.LoadSceneAsync("GameOver");
                am.startPreAd();
                canTakeDamage = false;
            }
            else
            {
                HandleMudarSlider(healthPoints);
                slider.value = healthPoints;
                damageCooldown = 1f;
            }


        }

    }
}