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

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        if (moveHorizontal == 0 && moveVertical == 0 && movement.x != 0 || movement.y != 0) {
            lastMovement = movement;
        }

        movement = new(moveHorizontal, moveVertical);

        anim.SetFloat("speedX", movement.x);
        anim.SetFloat("speedY", movement.y);
        anim.SetFloat("moveMagnitude", movement.magnitude);
        anim.SetFloat("lastMoveX", lastMovement.x);
        anim.SetFloat("lastMoveY", lastMovement.y);

        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * movement.normalized);

        float attack = Input.GetAxisRaw("Fire1");
        if (attack > 0 && Time.time - lastMeleeAtack > meleeAttackCooldown) {
            lastMeleeAtack = Time.time;
            Attack();
        }
    }

    void Update() {
        float interact = Input.GetAxisRaw("Interact");
        if (interact > 0 && Time.time - lastInteraction > interactCooldown)
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
                    // para a boss music presente no bm
                    if (bm.bossMusicSource != null)
                    {
                        bm.bossMusicSource.Stop();
                    }
                    else
                    {
                        Debug.LogError("Boss music source not found.");
                    }
                    if (musicmanagerAudioSource != null)
                    {
                        musicmanagerAudioSource.Play();
                    }
                    else
                    {
                        Debug.LogError("Music manager audio source not found.");
                    }


                    sceneToLoad = "Lobby";
                    bm.boss1CanSpawn = false;
                    bm.timePassed = Time.time;
                    bm.bossesKilled += 1;
                    bm.imageToChange.GetComponent<Image>().sprite = bm.bossAproachingIcon0quarto;

                    Health h = GetComponent<Health>();
                    h.healthPoints = h.maxHealthPoints;
                    h.TomarDano(0);

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

    void Attack() {
        anim.SetTrigger("attack");
    }

    void EnableAttackHitBox() {
        speed = 0f;
        Vector2 offset = new(0f, 0f);

        if (Math.Abs(movement.magnitude) > 0) {
            if (movement.x > 0) {
                offset = new(attackHitBoxOffset, 0f);
                attackHitBox.transform.localScale = (Vector3) new(1, 2);
            } else if (movement.x < 0) {
                offset = new(-attackHitBoxOffset, 0f);
                attackHitBox.transform.localScale = (Vector3) new(1, 2);
            } else if (movement.y > 0) {
                offset = new(0f, attackHitBoxOffset);
                attackHitBox.transform.localScale = (Vector3) new(2, 1);
            } else {
                offset = new(0f, -attackHitBoxOffset);
                attackHitBox.transform.localScale = (Vector3) new(2, 1);
            }
        } else {
            if (lastMovement.x > 0) {
                offset = new(attackHitBoxOffset, 0f);
                attackHitBox.transform.localScale = (Vector3) new(1, 2);
            } else if (lastMovement.x < 0) {
                offset = new(-attackHitBoxOffset, 0f);
                attackHitBox.transform.localScale = (Vector3) new(1, 2);
            } else if (lastMovement.y > 0) {
                offset = new(0f, attackHitBoxOffset);
                attackHitBox.transform.localScale = (Vector3) new(2, 1);
            } else {
                offset = new(0f, -attackHitBoxOffset);
                attackHitBox.transform.localScale = (Vector3) new(2, 1);
            }
        }

        attackHitBox.transform.position = (Vector2) transform.position + offset;
        attackHitBox.SetActive(true);
    }

    void DisableAttackHitBox() {
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

        if (other.CompareTag("PortalLobby")) {
            canUsePortalLobby = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("PortalLobby")) {
            canUsePortalLobby = false;
        }
    }
}
