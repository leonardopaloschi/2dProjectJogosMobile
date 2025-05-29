using UnityEngine;

public class ShotMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 2f;
    public float lifetime = 5f;
    public int damage = 1;
    public Vector2 moveDir = new Vector2(0f, 0f);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * moveDir.normalized);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyBase e = collision.GetComponent<EnemyBase>();
            e.TakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Breakable"))
        {
            Breakable b = collision.GetComponent<Breakable>();
            b.TakeDamage();
            Destroy(gameObject);
        }
        if (collision.CompareTag("Boss"))
        {
            Debug.Log("Hit a boss");
            // Tenta causar dano em qualquer tipo de boss existente
            skeletonBossScript boss1 = collision.gameObject.GetComponent<skeletonBossScript>();
            if (boss1 != null)
            {
                boss1.takeDamage(damage);
                return; // Se o boss1 for encontrado, não tenta o boss2
            }

            skeletonBossScript2 boss2 = collision.gameObject.GetComponent<skeletonBossScript2>();
            if (boss2 != null)
            {
                boss2.takeDamage(damage);
                return; // Se o boss2 for encontrado, não tenta o goblin
            }

            GoblinBoss goblinBoss = collision.gameObject.GetComponent<GoblinBoss>();
            if (goblinBoss != null)
            {
                goblinBoss.takeDamage(damage);
                return; // Se o goblinBoss for encontrado, não tenta o boss1 ou boss2
            }
            Destroy(gameObject);
        }
        
    }
}
