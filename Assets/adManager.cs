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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startPreAd()
    {
        if (!hasWatched)
        {
            preAdPanel.SetActive(true);

            StartCoroutine("WaitAndSkip");

            hasWatched = true;
        }
        else
        {
            SceneManager.LoadSceneAsync("GameOver");
        }
    }

    IEnumerator WaitAndSkip()
    {
        yield return new WaitForSeconds(10f);
        if (!watchAd)
        {
            SceneManager.LoadSceneAsync("GameOver");
        }
    }

    public void ShowAd()
    {
        watchAd = true;

        adPanel.SetActive(true);

        preAdPanel.SetActive(false);

        StartCoroutine("WaitToAllowSkipping");
    }

    IEnumerator WaitToAllowSkipping()
    {
        yield return new WaitForSeconds(5f);
        GameObject skipButton = GameObject.FindGameObjectWithTag("skipButton");
        if (skipButton != null)
        {
            skipButton.SetActive(true);
        }
        else
        {
            adPanel.SetActive(false);
        }
    }

    public void skipAd()
    {
        adPanel.SetActive(false);
        preAdPanel.SetActive(false);
        playerHealth.healthPoints = playerHealth.maxHealthPoints;
        playerHealth.HandleMudarSlider(playerHealth.healthPoints);
    }

}
