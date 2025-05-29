using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{

    public GameObject musicManager; // Musica do boss
    public AudioSource bossMusicSource; // Musica do boss
    public AudioClip bossMusic; // Musica do boss

    public GameObject imageToChange; // Imagem na UI para mudar quando o Boss estiver quase spawnando
    private float timeToChangeIcon1; // Quanto tempo depois de comecar a fase o icone deve mudar
    private float timeToChangeIcon2; // Quanto tempo depois de comecar a fase o icone deve mudar
    private float timeToChangeIcon3; // Quanto tempo depois de comecar a fase o icone deve mudar
    private float timeToChangeIcon4; // Quanto tempo depois de comecar a fase o icone deve mudar
    public Sprite bossAproachingIcon0quarto;
    public Sprite bossAproachingIcon1quarto; // Sprite para o qual o icone deve mudar para indicar que o boss esta quase spawnando
    public Sprite bossAproachingIcon2quarto; // Sprite para o qual o icone deve mudar para indicar que o boss esta quase spawnando
    public Sprite bossAproachingIcon3quarto; // Sprite para o qual o icone deve mudar para indicar que o boss esta quase spawnando
    public Sprite bossAproachingIcon4quarto; // Sprite para o qual o icone deve mudar para indicar que o boss esta quase spawnando

    [SerializeField] private float timeToSpawnBoss1 = 90f; // Quanto tempo depois de comecar a fase o boss deve spawnar
    public float timePassed; // Quanto tempo passou desde o comeco da fase
    public bool boss1CanSpawn = false; // Se o boss pode spawnar
    [SerializeField] private GameObject boss1Prefab; // Prefab do boss
    [SerializeField] private GameObject boss2Prefab; // Prefab do boss
    [SerializeField] private GameObject boss3Prefab; // Prefab do boss

    [SerializeField] private GameObject portalPrefab;

    public int bossesKilled = 0;
    public List<GameObject> bossPrefabs;

    void Start()
    {
        timePassed = Time.time;

        timeToChangeIcon1 = timeToSpawnBoss1 / 4;
        timeToChangeIcon2 = 2 * timeToSpawnBoss1 / 4;
        timeToChangeIcon3 = 3 * timeToSpawnBoss1 / 4;
        timeToChangeIcon4 = 4 * timeToSpawnBoss1 / 4;

        musicManager = GameObject.Find("MusicManager");
        if (musicManager != null)
        {
            bossMusicSource = musicManager.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogError("MusicManager not found in the scene.");
        }

        bossPrefabs = new List<GameObject>() { boss1Prefab, boss2Prefab, boss3Prefab };
    }

    // Update is called once per frame
    void Update()
    {
        if (boss1CanSpawn)
        {

            if (Time.time - timePassed > timeToSpawnBoss1)
            {
                if (musicManager != null && bossMusicSource != null)
                {
                    bossMusicSource.clip = bossMusic;
                    bossMusicSource.Play();
                }
                else
                {
                    Debug.LogError("MusicManager or AudioSource not found in the scene.");
                }
                boss1CanSpawn = false;
                GameObject boss = Instantiate(bossPrefabs[bossesKilled]);
                SceneManager.MoveGameObjectToScene(boss, SceneManager.GetSceneAt(1));
                GameObject[] teleports = GameObject.FindGameObjectsWithTag("TeleportZone");
                foreach (GameObject obj in teleports)
                {
                    obj.GetComponent<RoomTeleport>().enabled = false;
                    obj.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 255f);
                    obj.GetComponent<BoxCollider2D>().isTrigger = false;
                }
            }
            if (Time.time - timePassed > timeToChangeIcon4)
            {
                Image i = imageToChange.GetComponent<Image>();
                i.sprite = bossAproachingIcon4quarto;
            }
            else if (Time.time - timePassed > timeToChangeIcon3)
            {
                Image i = imageToChange.GetComponent<Image>();
                i.sprite = bossAproachingIcon3quarto;
            }
            else if (Time.time - timePassed > timeToChangeIcon2)
            {
                Image i = imageToChange.GetComponent<Image>();
                i.sprite = bossAproachingIcon2quarto;
            }
            else if (Time.time - timePassed > timeToChangeIcon1)
            {
                Image i = imageToChange.GetComponent<Image>();
                i.sprite = bossAproachingIcon1quarto;
            }
        }
    }

    public void SpawnPortal()
    {
        GameObject portal = Instantiate(portalPrefab, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(portal, SceneManager.GetSceneAt(1));
    }
}
