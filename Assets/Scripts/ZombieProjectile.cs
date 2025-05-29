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

            // Destr�i o proj�til ap�s atingir o jogador
            Destroy(gameObject);
        }

        // (Opcional) destr�i se atingir qualquer outro objeto com colis�o
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
