using UnityEngine;

public class skeletonBossScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created]

    private float attackCooldown = 3.0f;
    private float attackTimer = 0.0f;

    private Animator animator;
    private GameObject player;
    public float speed = 2.0f;

    public float xSpeed = 0.0f;
    public float ySpeed = 0.0f;
    public float speedMagnitude = 0.0f;

    private int attackType = -1;

    public int health = 20;
    public int maxHealth = 20;

    private bool isDead = false;

    public AudioClip attack1Sound;
    public AudioClip attack2Sound;
    public AudioClip attackSpecialSound;
    public AudioClip takeDamageSound;

    private AudioSource audioSource;


    public float distanceFromPlayer = 1000f;
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found on the boss.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("distanceFromPlayer: " + distanceFromPlayer);
        if (isDead)
        {
            return;
        }

        distanceFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);

        if (distanceFromPlayer < 50f)
        {
            if (attackTimer <= 0.0f)
            {

                if (distanceFromPlayer < 3f)
                {
                    attackType = chooseAttack();
                    attackTimer = attackCooldown;
                }
                else
                {
                    attackType = 2;
                    attackTimer = attackCooldown;
                }




                if (attackType == 0)
                {
                    animator.SetTrigger("attack1");
                    audioSource.PlayOneShot(attack1Sound);

                }
                else if (attackType == 1)
                {
                    animator.SetTrigger("attack2");
                    audioSource.PlayOneShot(attack2Sound);
                    audioSource.PlayOneShot(attack2Sound);
                    audioSource.PlayOneShot(attack2Sound);

                }
                else if (attackType == 2)
                {
                    animator.SetTrigger("attackSpecial");
                    audioSource.PlayOneShot(attackSpecialSound);
                }

                // Reset attack type after attacking
                attackType = -1;

            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
            followPlayer();
            updateAnimation();

        }
        else
        {
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", 0);
            animator.SetFloat("moveMagnitude", 0);
        }

    }

    private void followPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        xSpeed = direction.x;
        ySpeed = direction.y;
        speedMagnitude = Mathf.Sqrt(xSpeed * xSpeed + ySpeed * ySpeed);


        this.transform.position += new Vector3(xSpeed, ySpeed, 0) * speed * Time.deltaTime;
    }

    private void updateAnimation()
    {
        animator.SetFloat("moveX", xSpeed);
        animator.SetFloat("moveY", ySpeed);
        animator.SetFloat("moveMagnitude", speedMagnitude);
    }

    public void takeDamage(int damage)
    {
        Debug.Log("Boss took damage: " + damage);
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            animator.SetBool("isDead", true);
            SpawnPortal();
            Destroy(gameObject, 5.0f); // Destroy the object after 2 seconds
        }
        else
        {
            animator.SetTrigger("takeDamage");
        }
    }

    private int chooseAttack()
    {
        // choose seed based on time
        System.Random rand = new System.Random(System.DateTime.Now.Millisecond);
        int attackType = Random.Range(0, 2); // Randomly choose an attack type (0, 1, or 2)
        return attackType;
    }

    public void SpawnPortal()
    {
        BossManager bm = GameObject.FindGameObjectWithTag("BossManager").GetComponent<BossManager>();
        bm.SpawnPortal();
    }
}
