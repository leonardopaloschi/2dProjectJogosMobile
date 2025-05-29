using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void ChooseCharacter()
    {
        SceneManager.LoadSceneAsync("PlayerSelect");

        GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
        if (gm)
        {
            Destroy(gm);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void GoToMenu()
    {
        GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
        Destroy(gm);
        SceneManager.LoadSceneAsync(0);
    }
}