using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Vector2 movement;
    public Vector2 lastMovement;
    [SerializeField] public float maxSpeed = 5f;
    public float speed;
    public GameObject coinPrefab;

    private GameObject roomManager;

    [SerializeField] private GameObject attackHitBox;
    [SerializeField] private float attackHitBoxOffset = 1f;
    private float lastMeleeAtack;
    private float meleeAttackCooldown = 0.5f;

    private float interactCooldown = 1f;
    private float lastInteraction;
    public bool canUsePortalLobby = false;

    void Awake()
    {
        roomManager = GameObject.FindGameObjectWithTag("RoomManager");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Health h = GetComponent<Health>();
        h.slider = GameObject.FindGameObjectWithTag("SliderVidaUI").GetComponent<Slider>();
        h.healthTxt = GameObject.FindGameObjectWithTag("TextoVidaUI").GetComponent<TMP_Text>();
        h.slider.maxValue = h.maxHealthPoints;
        h.HandleMudarSlider(h.maxHealthPoints);
        speed = maxSpeed;
        lastMeleeAtack = Time.time;
        lastInteraction = Time.time;
    }

    void FixedUpdate()
    {
        anim.SetFloat("speedX", movement.x);
        anim.SetFloat("speedY", movement.y);
        anim.SetFloat("moveMagnitude", movement.magnitude);

        if (movement != Vector2.zero)
        {
            lastMovement = movement;
        }

        anim.SetFloat("lastMoveX", lastMovement.x);
        anim.SetFloat("lastMoveY", lastMovement.y);

        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * movement.normalized);
    }



    public void SetMovement(Vector2 dir)
    {
        movement = dir;
    }

    public void TryInteract()
    {
        if (Time.time - lastInteraction > interactCooldown)
        {
            if (canUsePortalLobby)
            {
                string sceneToLoad;
                BossManager bm = GameObject.FindGameObjectWithTag("BossManager").GetComponent<BossManager>();
                AudioSource musicmanagerAudioSource = GameObject.FindGameObjectWithTag("musicManager").GetComponent<AudioSource>();

                if (SceneManager.GetSceneAt(1).name.Equals("Lobby"))
                {
                    sceneToLoad = "inicial";
                    bm.boss1CanSpawn = true;
                    bm.timePassed = Time.time;
                    bm.imageToChange.GetComponent<Image>().sprite = bm.bossAproachingIcon0quarto;

                    Light2D l = GetComponentInChildren<Light2D>();
                    l.enabled = true;
                }
                else
                {
                    if (bm.bossMusicSource != null) bm.bossMusicSource.Stop();
                    else Debug.LogError("Boss music source not found.");

                    if (musicmanagerAudioSource != null) musicmanagerAudioSource.Play();
                    else Debug.LogError("Music manager audio source not found.");

                    sceneToLoad = "Lobby";
                    bm.boss1CanSpawn = false;
                    bm.timePassed = Time.time;
                    bm.bossesKilled += 1;
                    bm.imageToChange.GetComponent<Image>().sprite = bm.bossAproachingIcon0quarto;

                    Health h = GetComponent<Health>();
                    h.healthPoints = h.maxHealthPoints;
                    h.HandleMudarSlider(h.healthPoints);

                    Light2D l = GetComponentInChildren<Light2D>();
                    l.enabled = false;
                }

                if (bm.bossesKilled == 3)
                {
                    SceneManager.LoadSceneAsync("WinScreen");
                }
                else
                {
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1).name);
                    SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
                }

                canUsePortalLobby = false;
            }

            lastInteraction = Time.time;
        }
    }

    public void Attack()
    {
        if (Time.time - lastMeleeAtack < meleeAttackCooldown) return;

        lastMeleeAtack = Time.time;
        anim.SetTrigger("attack");
    }

    void EnableAttackHitBox()
    {
        speed = 0f;
        Vector2 offset = Vector2.zero;

        if (movement != Vector2.zero)
        {
            if (movement.x > 0)
            {
                offset = new Vector2(attackHitBoxOffset, 0f);
                attackHitBox.transform.localScale = new Vector3(1, 2);
            }
            else if (movement.x < 0)
            {
                offset = new Vector2(-attackHitBoxOffset, 0f);
                attackHitBox.transform.localScale = new Vector3(1, 2);
            }
            else if (movement.y > 0)
            {
                offset = new Vector2(0f, attackHitBoxOffset);
                attackHitBox.transform.localScale = new Vector3(2, 1);
            }
            else
            {
                offset = new Vector2(0f, -attackHitBoxOffset);
                attackHitBox.transform.localScale = new Vector3(2, 1);
            }
        }
        else
        {
            if (lastMovement.x > 0) offset = new Vector2(attackHitBoxOffset, 0f);
            else if (lastMovement.x < 0) offset = new Vector2(-attackHitBoxOffset, 0f);
            else if (lastMovement.y > 0) offset = new Vector2(0f, attackHitBoxOffset);
            else offset = new Vector2(0f, -attackHitBoxOffset);

            attackHitBox.transform.localScale = (Mathf.Abs(lastMovement.x) > 0)
                ? new Vector3(1, 2)
                : new Vector3(2, 1);
        }

        attackHitBox.transform.position = (Vector2)transform.position + offset;
        attackHitBox.SetActive(true);
    }

    void DisableAttackHitBox()
    {
        attackHitBox.SetActive(false);
        speed = maxSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            maxSpeed += 1f;
            speed = maxSpeed;

            RoomManager rm = roomManager.GetComponent<RoomManager>();
            string currentRoom = SceneManager.GetSceneAt(1).name;
            rm.roomObjects[currentRoom].Add(other.gameObject.name, false);

            Destroy(other.gameObject);
        }

        if (other.CompareTag("PortalLobby"))
        {
            canUsePortalLobby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PortalLobby"))
        {
            canUsePortalLobby = false;
        }
    }
}
