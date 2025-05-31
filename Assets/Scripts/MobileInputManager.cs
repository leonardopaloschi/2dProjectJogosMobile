// MobileInputManager.cs � atualizado para exibir controles SOMENTE na cena "Player" e quando pauseMenu est� desativado
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MobileInputManager : MonoBehaviour
{
    public FixedJoystick movementJoystick; // do Joystick Pack
    public GameObject attackButton;
    public GameObject interactButton;
    public GameObject pauseButton;

    public GameObject pauseMenu; // atribuir no inspetor (painel de pause)

    public PlayerMovement playerMovement;
    public PlayerSkeletonAttack skeletonAttack;
    public Collectable collectable;
    public PauseManager pauseManager;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();   
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMobileControlsVisibility();
    }

    private void Update()
    {
        UpdateMobileControlsVisibility();


        if (playerMovement != null && movementJoystick != null && movementJoystick.gameObject.activeSelf)
        {
            Vector2 moveDir = new Vector2(movementJoystick.Horizontal, movementJoystick.Vertical);
            playerMovement.SetMovement(moveDir);
        }
    }

    private void UpdateMobileControlsVisibility()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // S� ativa se estiver na cena Player e pauseMenu estiver inativo
        if (sceneName == "Player" && (pauseMenu == null || !pauseMenu.activeSelf))
        {
            ToggleMobileControls(true);
        }
        else
        {
            ToggleMobileControls(false);
        }
    }

    private void ToggleMobileControls(bool state)
    {
        if (movementJoystick != null)
            movementJoystick.gameObject.SetActive(state);
        if (attackButton != null)
            attackButton.SetActive(state);
        if (interactButton != null)
            interactButton.SetActive(state);
        if (pauseButton != null)
            pauseButton.SetActive(state);
    }

    public void OnAttackButtonPressed()
    {
        Debug.Log("Bot�o de ataque pressionado.");

        Debug.Log(playerMovement);
        if (playerMovement != null)
        {
            Debug.Log("AAAAAAA");
            playerMovement.Attack();
        }

        if (skeletonAttack != null)
        {
            skeletonAttack.SendMessage("Shoot", SendMessageOptions.DontRequireReceiver);
        }
    }


    public void OnInteractButtonPressed()
    {
        Debug.Log("Bot�o de interagir pressionado.");

        if (playerMovement != null)
        {
            playerMovement.TryInteract();
        }
    }

    public void OnPauseButtonPressed()
    {
        if (pauseManager != null)
        {
            if (pauseManager.IsPaused()) pauseManager.ResumeGame();
            else pauseManager.PauseGame();
        }
    }
}
