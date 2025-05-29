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
        if (damageCooldown == 0)
        {
            healthPoints -= dano;

            if (healthPoints <= 0)
            {
                //SceneManager.LoadSceneAsync("GameOver");
                am.startPreAd();
            }

            HandleMudarSlider(healthPoints);
            slider.value = healthPoints;
            damageCooldown = 1f;
        }

    }
}