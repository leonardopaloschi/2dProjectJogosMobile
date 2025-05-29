using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int meleeDamage = 3;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyBase e = collision.gameObject.GetComponent<EnemyBase>();
            if (e != null)
            {
                e.TakeDamage(meleeDamage);
            }
        }
        else if (collision.CompareTag("Boss"))
        {
            Debug.Log("Hit a boss");
            // Tenta causar dano em qualquer tipo de boss existente
            skeletonBossScript boss1 = collision.gameObject.GetComponent<skeletonBossScript>();
            if (boss1 != null)
            {
                boss1.takeDamage(meleeDamage);
                return; // Se o boss1 for encontrado, não tenta o boss2
            }

            skeletonBossScript2 boss2 = collision.gameObject.GetComponent<skeletonBossScript2>();
            if (boss2 != null)
            {
                boss2.takeDamage(meleeDamage);
                return; // Se o boss2 for encontrado, não tenta o goblin
            }

            GoblinBoss goblinBoss = collision.gameObject.GetComponent<GoblinBoss>();
            if (goblinBoss != null)
            {
                goblinBoss.takeDamage(meleeDamage);
                return; // Se o goblinBoss for encontrado, não tenta o boss1 ou boss2
            }
        }
        else if (collision.CompareTag("Breakable"))
        {
            Debug.Log("Hit a breakable object");
            Breakable b = collision.gameObject.GetComponent<Breakable>();
            if (b != null)
            {
                b.TakeDamage();
            }
        }
    }
}
