using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ZombieBehavior : EnemyBase
{
    public GameObject projectilePrefab;
    public GameObject trailPrefab;
    public float shootSpeed = 8f;
    public float trailSpawnInterval = 1f;

    private float lastTrailTime = 0f;
    private AudioClip shootSound;

    protected override void Start()
    {
        base.Start();
        shootSound = Resources.Load<AudioClip>("Sounds/zombieShoot");
    }

    protected override void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Ataque: s� dispara se dentro do alcance, cooldown ok e n�o estiver atacando
        if (!isAttacking && distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            lockedAttackDirection = (player.position - transform.position).normalized;
            lockedAnimDirection = lockedAttackDirection;

            anim.SetFloat("moveX", lockedAnimDirection.x);
            anim.SetFloat("moveY", lockedAnimDirection.y);
            anim.SetFloat("moveMagnitude", lockedAnimDirection.magnitude);

            anim.SetTrigger("attack");
            currentSpeed = 0f; // para de se mover s� durante o ataque
            return; // garante que ele n�o ande nesse frame
        }

        // Movimento: continua seguindo o jogador se n�o estiver atacando
        if (!isAttacking)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(currentSpeed * Time.deltaTime * direction);

            anim.SetFloat("moveX", direction.x);
            anim.SetFloat("moveY", direction.y);
            anim.SetFloat("moveMagnitude", direction.magnitude);
        }

        // Rastro t�xico
        if (Time.time - lastTrailTime >= trailSpawnInterval)
        {
            SpawnTrail();
            lastTrailTime = Time.time;
        }
    }

    protected override IEnumerator PerformDelayedAttack()
    {
        if (isDead) yield break;

        isAttacking = true;

        // Toca som
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Cria proj�til
        if (projectilePrefab != null)
        {
            Vector2 shootPosition = (Vector2)transform.position + lockedAttackDirection * 0.5f;
            GameObject proj = Instantiate(projectilePrefab, shootPosition, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(proj, SceneManager.GetSceneAt(1));
            Debug.Log("Criou proj�til");

            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;
                rb.linearVelocity = lockedAttackDirection * shootSpeed;
            }
        }

        yield return null;
    }

    void SpawnTrail()
    {
        if (trailPrefab != null)
        {
            Instantiate(trailPrefab, transform.position, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(trailPrefab, SceneManager.GetSceneAt(1));
        }
    }

    public override void EndAttackAnimation()
    {
        base.EndAttackAnimation(); // volta a andar
    }
}
