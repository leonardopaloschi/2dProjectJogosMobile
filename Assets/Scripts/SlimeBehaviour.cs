using UnityEngine;

public class SlimeBehavior : EnemyBase
{
    private AudioClip attackSound;
    private AudioClip deathSound;

    protected override void Start()
    {
        base.Start();
        attackSound = Resources.Load<AudioClip>("Sounds/slimeAttack");
        deathSound = Resources.Load<AudioClip>("Sounds/slimeDeath");
    }

    protected override System.Collections.IEnumerator PerformDelayedAttack()
    {
        if (attackSound != null && audioSource != null)
            audioSource.PlayOneShot(attackSound);

        int layerMask = ~LayerMask.GetMask("Enemy");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, layerMask);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<Health>().TomarDano(meleeDamage);
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
