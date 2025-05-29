using UnityEngine;

public class ZombieProjectile : MonoBehaviour
{
    public int damage = 6;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Tenta acessar o script de vida do player
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TomarDano(damage);
            }

            // Destrói o projétil após atingir o jogador
            Destroy(gameObject);
        }

        // (Opcional) destrói se atingir qualquer outro objeto com colisão
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
