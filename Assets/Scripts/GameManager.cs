using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerWizardPrefab;
    [SerializeField] private GameObject playerSkeletonPrefab;
    public GameObject playerPrefabSelected;

    [SerializeField] private RuntimeAnimatorController playerWizardDeathAnimator;
    [SerializeField] private RuntimeAnimatorController playerSkeletonDeathAnimator;
    public RuntimeAnimatorController playerDeathAnimatorSelected;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SelectWizard()
    {
        playerPrefabSelected = playerWizardPrefab;
        playerDeathAnimatorSelected = playerWizardDeathAnimator;
        GameObject audio = GameObject.FindGameObjectWithTag("audioinicial");
        if (audio)
        {
            Destroy(audio);
        }
        PlayGame();
    }

    public void SelectSkeleton()
    {
        playerPrefabSelected = playerSkeletonPrefab;
        playerDeathAnimatorSelected = playerSkeletonDeathAnimator;
        GameObject audio = GameObject.FindGameObjectWithTag("audioinicial");
        if (audio)
        {
            Destroy(audio);
        }
        PlayGame();
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Player");
        SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive);
    }

}
