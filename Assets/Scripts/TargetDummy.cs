using System.Collections;
using UnityEngine;

public class TargetDummy : EnemyBase
{
    public AudioSource hitSound;
    public AudioClip hitClip;

    protected override void Start()
    {
        base.Start();
        hitSound = GetComponent<AudioSource>();
        if (hitSound == null)
        {
            Debug.LogError("Hit sound not assigned.");
        }
        if (hitClip == null)
        {
            Debug.LogError("Hit clip not assigned.");
        }
    }

    override public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        isAttacking = false;
        currentSpeed = maxSpeed;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            anim.SetTrigger("takeDamage");
            if (hitSound != null && hitClip != null)
            {
                hitSound.PlayOneShot(hitClip);
            }
        }
    }

    protected override IEnumerator PerformDelayedAttack() { yield break; }
}
