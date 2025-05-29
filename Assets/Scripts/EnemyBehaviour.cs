using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float maxSpeed = 2f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public int maxHealth = 3;
    public float meleeDamage = 5f;

    private float currentSpeed;
    private int currentHealth;
    private Transform player;
    private float lastAttackTime;
    private AudioSource audioSource;

    private AudioClip attackSound;
    private AudioClip deathSound;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 direction;
    private Vector2 lastMovement;

    private bool isAttacking = false;
    private Vector2 lockedAttackDirection;
    private Vector2 lockedAnimDirection;


    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        currentHealth = maxHealth;
        currentSpeed = maxSpeed;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        attackSound = Resources.Load<AudioClip>("Sounds/goblinAttack");
        deathSound = Resources.Load<AudioClip>("Sounds/goblinDeath");
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            isAttacking = false;
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(currentSpeed * Time.deltaTime * direction);

            if (!anim.GetBool("attack"))
            {
                anim.SetFloat("moveX", direction.x);
                anim.SetFloat("moveY", direction.y);
                anim.SetFloat("moveMagnitude", direction.magnitude);
            }
        }
        else
        {
            if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
            {
                isAttacking = true;
                lockedAttackDirection = (player.position - transform.position).normalized;
                lockedAnimDirection = lockedAttackDirection;

                anim.SetFloat("moveX", lockedAnimDirection.x);
                anim.SetFloat("moveY", lockedAnimDirection.y);
                anim.SetFloat("moveMagnitude", lockedAnimDirection.magnitude);

                anim.SetBool("attack", true);
                currentSpeed = 0f;

                StartCoroutine(PerformDelayedAttack(0.5f));
                lastAttackTime = Time.time;
            }
        }
    }


    System.Collections.IEnumerator PerformDelayedAttack(float delay)
    {
        yield return new WaitForSeconds(delay);

        int layerMask = ~LayerMask.GetMask("Enemy");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lockedAttackDirection, attackRange, layerMask);

        if (attackSound != null && audioSource != null)
            audioSource.PlayOneShot(attackSound);

        if (hit.collider != null)
        {
            Debug.Log("Raycast acertou: " + hit.collider.name);
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<Health>().TomarDano(meleeDamage);
            }
        }
        else
        {
            Debug.Log("Raycast não acertou nada");
        }
    }

    public void EndAttackAnimation()
    {
        anim.SetBool("attack", false);
        currentSpeed = maxSpeed;
        isAttacking = false;
        Debug.Log("Passou aqui");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Inimigo morreu!");

        if (deathSound != null && audioSource != null)
            audioSource.PlayOneShot(deathSound);

        Destroy(gameObject, 0.5f);
    }
}
