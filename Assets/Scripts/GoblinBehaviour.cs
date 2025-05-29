using UnityEngine;

public class GoblinBehavior : EnemyBase
{
    private AudioClip attackSound;
    private AudioClip deathSound;

    protected override void Start()
    {
        base.Start();
        attackSound = Resources.Load<AudioClip>("Sounds/goblinAttack");
        deathSound = Resources.Load<AudioClip>("Sounds/goblinDeath");
    }

    protected override System.Collections.IEnumerator PerformDelayedAttack()
    {
        // Debug.Log($"Inimigo {gameObject.name} executou ataque em {Time.time}");

        if (attackSound != null && audioSource != null)
            audioSource.PlayOneShot(attackSound);

        int layerMask = ~LayerMask.GetMask("Enemy");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lockedAttackDirection, attackRange, layerMask);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Health h = hit.collider.GetComponent<Health>();
            h.TomarDano(meleeDamage);
            if (h.isKnockbackActive) {
                rb.MovePosition(-lockedAnimDirection * h.knockbackAmount + rb.position);
            }
        }
        yield break;
    }
        
    protected override void Die()
    {
        if (deathSound != null && audioSource != null)
            audioSource.PlayOneShot(deathSound);

        base.Die();
    }

    new public void EndAttackAnimation()
    {
        base.EndAttackAnimation();
    }
}
