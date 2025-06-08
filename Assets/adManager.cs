using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class adManager : MonoBehaviour
{
    public GameObject adPanel;
    public GameObject preAdPanel;
    private bool watchAd = false;
    private bool hasWatched = false;
    public Health playerHealth;

    private bool isPreAdActive = false;

    void Start()
    {
        // Inicializa o painel de anúncio e o painel pré-anúncio
        adPanel.SetActive(false);
        preAdPanel.SetActive(false);
    }

    void Update()
    {
        if (isPreAdActive)
        {
            //
        }

    }

    public void startPreAd()
    {
        preAdPanel.SetActive(true);
        isPreAdActive = true;
    }

    public void ShowhAd()
    {
        if (isPreAdActive)
        {
            preAdPanel.SetActive(false);
            adPanel.SetActive(true);
            hasWatched = true;
            isPreAdActive = false;
        }
        StopCoroutine(waitForWatch());
        StartCoroutine(waitForWatch());
    }

    IEnumerator waitForWatch()
    {
        yield return new WaitForSeconds(5f);
        attemptGameOver();
    }
    
    public void attemptGameOver()
    {
        if (hasWatched)
        {
            playerHealth.ResetHealth();
            preAdPanel.SetActive(false);
            adPanel.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene("GameOver");
        }
        
    }
}
