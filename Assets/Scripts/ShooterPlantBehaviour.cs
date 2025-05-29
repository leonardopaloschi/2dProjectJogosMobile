using UnityEngine;
using UnityEngine.SceneManagement;

public class ShooterPlantBehaviour : EnemyBase
{
    public GameObject projectilePrefab;
    public float shootSpeed = 10f;
    public float retreatRange = 6f;

    private AudioClip shootSound;
    private bool atacando = false;
    protected override void Start()
    {
        base.Start();
        shootSound = Resources.Load<AudioClip>("Sounds/plantShoot");
    }

    protected override void Update()
    {
        if (player == null || isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            //isAttacking = true;
            atacando = true;
            lockedAttackDirection = directionToPlayer;
            lockedAnimDirection = directionToPlayer;

            anim.SetFloat("moveX", lockedAnimDirection.x);
            anim.SetFloat("moveY", lockedAnimDirection.y);

            anim.SetTrigger("attack");
            currentSpeed = 0f;
            lastAttackTime = Time.time;
        }
        else
        {
            if (atacando)
            {
                lockedAnimDirection = directionToPlayer;
                anim.SetFloat("moveX", lockedAnimDirection.x);
                anim.SetFloat("moveY", lockedAnimDirection.y);
                currentSpeed = 0f;
            }
            else
            {
                if (distance < retreatRange)
                {
                    Vector2 retreatDir = -directionToPlayer;
                    transform.position += (Vector3)(currentSpeed * Time.deltaTime * retreatDir);

                    anim.SetFloat("moveX", retreatDir.x);
                    anim.SetFloat("moveY", retreatDir.y);
                    anim.SetFloat("moveMagnitude", retreatDir.magnitude);
                }
                else
                {
                    anim.SetFloat("moveMagnitude", 0f);
                }
            }
        }

    }

    protected override System.Collections.IEnumerator PerformDelayedAttack()
    {
        if (isDead) yield break;

        if (shootSound != null && audioSource != null)
            audioSource.PlayOneShot(shootSound);

        if (projectilePrefab != null)
        {
            Vector2 shootPosition = (Vector2)transform.position + lockedAttackDirection * 0.5f;
            GameObject proj = Instantiate(projectilePrefab, shootPosition, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(proj, SceneManager.GetSceneAt(1));

            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;
                rb.linearVelocity = lockedAttackDirection * shootSpeed;
            }
        }
    }

    override public void EndAttackAnimation()
    {
        atacando = false;
        base.EndAttackAnimation();
    }
}
