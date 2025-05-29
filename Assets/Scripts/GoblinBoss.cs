using UnityEngine;

public class GoblinBoss : MonoBehaviour
{
    private float attackCooldown = 4.0f;
    private float attackTimer = 0.0f;

    private Animator animator;
    public GameObject player;
    public float speed = 2.0f;

    public float xSpeed = 0.0f;
    public float ySpeed = 0.0f;
    public float speedMagnitude = 0.0f;

    public int attackType = -1;

    public int health = 20;
    public int maxHealth = 20;

    private bool isDead = false;

    public float distanceFromPlayer = 1000f;

    private Collider2D bossCollider;
    private Vector2 lastKnownPlayerDirection = Vector2.right;


    // Prefabs diferentes para cada ataque
    public GameObject projectileAttack2Prefab;
    public GameObject projectileSpecialPrefab;

    public float projectileSpeed = 5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        bossCollider = GetComponent<Collider2D>();

        bossCollider.enabled = true;

        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }
        if (bossCollider == null)
        {
            Debug.LogError("Boss Collider2D not found.");
        }
    }

    void Update()
    {
        if (isDead) return;

        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (attackTimer <= 0.0f)
        {
            lastKnownPlayerDirection = (player.transform.position - transform.position).normalized;
            attackType = Random.Range(0, 3); // 0, 1 ou 2 aleat�rio
            attackTimer = attackCooldown;

            switch (attackType)
            {
                case 0:
                    animator.SetTrigger("attack1");
                    break;
                case 1:
                    animator.SetTrigger("attack2");
                    break;
                case 2:
                    animator.SetTrigger("attackSpecial");
                    break;
            }

            attackType = -1;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }

        followPlayer();
        updateAnimation();
    }

    private void followPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        xSpeed = direction.x;
        ySpeed = direction.y;
        speedMagnitude = direction.magnitude;

        transform.position += direction * speed * Time.deltaTime;
    }

    private void updateAnimation()
    {
        animator.SetFloat("moveX", xSpeed);
        animator.SetFloat("moveY", ySpeed);
        animator.SetFloat("moveMagnitude", speedMagnitude);
    }

    public void takeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        Debug.Log("Boss took damage: " + damage);

        if (health <= 0)
        {
            isDead = true;
            animator.SetBool("isDead", true);
            Destroy(gameObject, 5.0f);
        }
        else
        {
            animator.SetTrigger("takeDamage");
        }
    }

    public void DisableCollider()
    {
        Debug.Log("Disabling collider");
        if (bossCollider != null)
        {
            bossCollider.enabled = false;
        }
    }

    public void EnableCollider()
    {
        Debug.Log("Enabling collider");
        if (bossCollider != null)
        {
            bossCollider.enabled = true;
        }
    }

    // Animation Event para o ataque 2
    public void LaunchProjectileAttack2()
    {
        LaunchProjectile(projectileAttack2Prefab);
    }

    // Animation Event para o ataque especial
    public void LaunchProjectileSpecial()
    {
        LaunchProjectile(projectileSpecialPrefab);
    }

    private void LaunchProjectile(GameObject prefab)
    {
        lastKnownPlayerDirection = (player.transform.position - transform.position).normalized;
        Vector2 spawnPos = transform.position;
        Vector2 direction = lastKnownPlayerDirection.normalized;

        GameObject projectile = Instantiate(prefab, spawnPos, Quaternion.identity);

        // Rotaciona o proj�til para apontar na dire��o do movimento
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * 5f; // ajuste a velocidade conforme necess�rio
        }
    }

    public void SpawnPortal()
    {
        BossManager bm = GameObject.FindGameObjectWithTag("BossManager").GetComponent<BossManager>();
        bm.SpawnPortal();
    }

}
