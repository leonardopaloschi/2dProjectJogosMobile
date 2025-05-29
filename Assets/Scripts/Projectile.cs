using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;
    public int damage = 1;

    private void Start()
    {
        // Destroi o projetil ap�s um tempo caso n�o colida com nada
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TomarDano(damage);
            }
            Destroy(gameObject); // Destroi a bolinha ap�s atingir o jogador
        }
        else if (!collision.isTrigger)
        {
            // Colidiu com parede ou objeto f�sico � destr�i o projetil
            Destroy(gameObject);
        }
    }
}
